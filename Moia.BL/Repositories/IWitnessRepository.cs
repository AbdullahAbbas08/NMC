using Moia.Shared.Models;

namespace Moia.BL.Repositories
{
    public interface IWitnessRepository : IRepository<Witness>
    {
        ViewerPagination<Witness> getWithPaginate(int page, int pageSize, string searchTerm);
    }


    public class WitnessRepository : Repository<Witness>, IWitnessRepository
    {
        private readonly IUnitOfWork uow;

        public WitnessRepository(IUnitOfWork _uow) : base(_uow)
        {
            uow = _uow;
        }

        public ViewerPagination<Witness> getWithPaginate(int page, int pageSize, string searchTerm)
        {
            IQueryable<Witness> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.Witness
               .Where(a => a.Name.Contains(searchTerm) ||
                            a.Identity.Contains(searchTerm) ||
                            a.Mobile.Contains(searchTerm)
                            );
            }
            else
            {
                myData = uow.DbContext.Witness;
            }
            myDataCount = myData.Count();
            ViewerPagination<Witness> viewerPagination = new ViewerPagination<Witness>();

            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new Witness
            {
                ID = x.ID,
                Name = x.Name,
                Mobile = x.Mobile,
                Identity = x.Identity,
            }).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

    }





}
