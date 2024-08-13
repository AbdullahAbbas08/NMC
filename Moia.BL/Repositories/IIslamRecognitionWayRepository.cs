using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.BL.Repositories
{
    public interface IIslamRecognitionWayRepository : IRepository<IsslamRecognition>
    {
        ViewerPagination<IsslamRecognitionDto> getWithPaginate(int page, int pageSize, string searchTerm);
    }

    public class IslamRecognitionWayRepository : Repository<IsslamRecognition>, IIslamRecognitionWayRepository
    {
        private readonly IUnitOfWork uow;

        public IslamRecognitionWayRepository(IUnitOfWork _uow) : base(_uow)
        {
            uow = _uow;
        }

        public ViewerPagination<IsslamRecognitionDto> getWithPaginate(int page, int pageSize, string searchTerm)
        {
            IQueryable<IsslamRecognition> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.IsslamRecognition
               .Where(a => a.Title.Contains(searchTerm) );
            }
            else
            {
                myData = uow.DbContext.IsslamRecognition;
            }
            myDataCount = myData.Count();
            ViewerPagination<IsslamRecognitionDto> viewerPagination = new ViewerPagination<IsslamRecognitionDto>();

            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new IsslamRecognitionDto
            {
                ID = x.ID,
                Title = x.Title
            }).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

    }
} 
