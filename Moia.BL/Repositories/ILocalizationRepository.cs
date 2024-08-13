using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;
using System.Text.Json;

namespace Moia.BL.Repositories
{
    public interface ILocalizationRepository : IRepository<Localization>
    {
        public string GetByKey(string KeyCode, string culture);
        public string GetJson(IEnumerable<Localization> myList);
        //public string GetJson(string culture, IEnumerable<Localization> myList);
        //public DateTime GetLastLocalizationUpdateTime(IEnumerable<Localization> myList);
        public IEnumerable<Localization> GetAllWithoutTracking();
        public bool updatTranslation(LocalizationDetailsDTO localization, int userId);
        public bool addTranslation(LocalizationDetailsDTO localization);
        public bool deleteTranslation(int id);
        public ViewerPagination<LocalizationDetailsDTO> getTranslations(int page, int pageSize, string searchTerm);
        public LocalizationDetailsDTO getTranslationsById(int Id);
        public bool addBulkTranslation(List<LocalizationDetailsDTO> localizations, int userId);
    }

    public class LocalizationRepository : Repository<Localization>, ILocalizationRepository
    {
        private readonly IUnitOfWork uow;

        public LocalizationRepository(IUnitOfWork uow) : base(uow)
        {
            this.uow = uow;
        }

        public IEnumerable<Localization> GetAllWithoutTracking()
        {
            try
            {
                var MyList = uow.DbContext.Localizations;
                if (MyList != null)
                {
                    return MyList.AsNoTracking().ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetByKey(string KeyCode, string culture)
        {
            string Name = "";
            if (culture.ToLower() == "ar")
            {
                Name = uow.DbContext.Localizations.Where(w => w.Key == KeyCode).FirstOrDefault()?.ValueAr;

            }
            else
            {
                Name = uow.DbContext.Localizations.Where(w => w.Key == KeyCode).FirstOrDefault()?.ValueEn;
            }

            return Name;
        }

        public bool updatTranslation(LocalizationDetailsDTO localization, int userId)
        {
            var oldOne = uow.DbContext.Localizations.Where(w => w.ID == localization.Id).FirstOrDefault();
            if (oldOne != null)
            {
                oldOne.ValueAr = string.IsNullOrEmpty(localization.ValueAr) ? oldOne.ValueAr : localization.ValueAr;
                oldOne.ValueEn = string.IsNullOrEmpty(localization.ValueEn) ? oldOne.ValueEn : localization.ValueEn;

                return true;
            }
            return false;
        }
        public bool deleteTranslation(int id)
        {
            var deletedOne = uow.DbContext.Localizations.Where(s => s.ID == id).FirstOrDefault();
            if (deletedOne != null)
            {
                uow.DbContext.Localizations.Remove(deletedOne);
                uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool addTranslation(LocalizationDetailsDTO localization)
        {
            var isExist = uow.DbContext.Localizations.FirstOrDefault(w => w.Key == localization.Key);
            //if (string.IsNullOrEmpty(localization.Key) || isExist)
            //{
            //    return false;
            //}
            if (isExist != null)
            {
                isExist.ValueAr = localization.ValueAr;
                uow.SaveChanges();
            }
            else
            {
                Localization Localization = new Localization();
                Localization.Key = localization.Key;
                Localization.ValueAr = string.IsNullOrEmpty(localization.ValueAr) ? null : localization.ValueAr;
                Localization.ValueEn = string.IsNullOrEmpty(localization.ValueEn) ? null : localization.ValueEn;
                //Localization.crea = DateTime.Now;
                //Localization.CreatedBy = userId == 0 ? null : userId;
                uow.DbContext.Localizations.Add(Localization);
                uow.SaveChanges();
            }
            return true;
        }

        public bool addBulkTranslation(List<LocalizationDetailsDTO> localizations, int userId)
        {
            List<Localization> LocalizationList = new List<Localization>();
            foreach (var localization in localizations)
            {
                Localization Localization = new Localization();
                Localization.Key = localization.Key;
                Localization.ValueAr = localization.ValueAr;
                Localization.ValueEn = localization.ValueEn;
                LocalizationList.Add(Localization);
            }
            uow.DbContext.Localizations.AddRange(LocalizationList);
            // _Context.SaveChanges();
            return true;
        }
        public LocalizationDetailsDTO getTranslationsById(int Id)
        {
            return uow.DbContext.Localizations.Where(a => a.ID == Id).Select(x => new LocalizationDetailsDTO
            {
                Id = x.ID,
                Key = x.Key,
                ValueAr = x.ValueAr,
                ValueEn = x.ValueEn,
            }).FirstOrDefault();
        }
        public ViewerPagination<LocalizationDetailsDTO> getTranslations(int page, int pageSize, string searchTerm)
        {
            IQueryable<Localization> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.Localizations
               .Where(a => a.Key.Contains(searchTerm) ||
                      a.ValueAr.Contains(searchTerm) ||
                      a.ValueEn.Contains(searchTerm)
                      );
            }
            else
            {
                myData = uow.DbContext.Localizations;
            }
            myDataCount = myData.Count();
            ViewerPagination<LocalizationDetailsDTO> viewerPagination = new ViewerPagination<LocalizationDetailsDTO>();

            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new LocalizationDetailsDTO
            {
                Id = x.ID,
                Key = x.Key,
                ValueAr = x.ValueAr,
                ValueEn = x.ValueEn,
            }).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

        public string GetJson(IEnumerable<Localization> myList)
        {
            var dictionary = myList.ToDictionary(
                x => x.Key,
                x => x.ValueAr
            );

            var jsonString = JsonSerializer.Serialize(dictionary, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });

            return jsonString;
        }

        //public string GetJson(string culture, IEnumerable<Localization> myList)
        //{
        //    var list = myList.Select(x => new {
        //        x.Key,
        //        Value = culture.ToLower().StartsWith("ar") ? x.ValueAr
        //        : culture.ToLower().StartsWith("en") ? x.ValueEn
        //        : x.ValueAr
        //    })
        //        .ToList();
        //    var result = "{" + string.Join(", ", list.Select(x => "\"" + x.Key + "\":\"" + x.Value + "\"")) + "}";
        //    return result;
        //}

        //public DateTime GetLastLocalizationUpdateTime(IEnumerable<Localization> myList)
        //{
        //    return DateTime.Parse(myList.Select(x => x.CreatedOn)
        //        .Union(myList.Select(x => x.UpdatedOn)).Max().GetValueOrDefault().ToString());
        //}

    }
}