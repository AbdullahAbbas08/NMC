using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Moia.BL.Repositories;
using Moia.Shared.Models;
using SmsIntegration;
using System.Drawing;
using System.Net.Http;

namespace Moia.BL
{
    public interface IUnitOfWork
    {
        string ContentRootPath { get; }
        string WebRootPath { get; }
        //IBackgroundJobClient BackgroundJobClient { get; }
        DatabaseContext DbContext { get; }
        int ExecuteSqlRaw(string sql, params object[] parameters);
        int SaveChanges();
        IMapper Mapper { get; }
        HttpContext HttpContext { get; }
        HttpClient HttpClient { get; }
        IHttpContextAccessor _httpContextAccessor { get; }
        IConfiguration Configuration { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IIntegrationRepository IntegrationRepository { get; }
        ILocalizationRepository LocalizationRepository { get; }
        ITokenStoreRepository TokenStoreRepository { get; }
        ISessionServices SessionServices { get; }
        IDataProtectRepository DataProtectRepository { get; }
        ISecurityRepository SecurityRepository { get; }
        IOptionsSnapshot<BearerTokensOptions> OptionsSnapshot { get; }
        IMailServices MailServices { get; }
        ISmsIntegrations SmsIntegrations { get; }
        //Task<byte[]> ConvertIFormFileToByteArray(IFormFile file);
        Task<byte[]> ConvertIFormFileToByteArray(IFormFile file, int targetWidth, int targetHeight);

        #region Repositories
        public IMuslimeRepository Muslime { get; }
        public IWitnessRepository Witness { get; }
        public ICommitteeRepository Committee { get; }
        public IUserRepository User { get; }
        public IIslamRecognitionWayRepository IslamRecognition { get; }
        public IReportRepository Report { get; }
        #endregion
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly string contentRootPath;
        private readonly string webRootPath;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly HttpContext httpContext;
        private readonly HttpContextAccessor _HttpContextAccessor;
        private readonly DatabaseContext db;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;
        IMailServices mailServices;
        ISmsIntegrations smsIntegrations;
        private readonly ISessionServices SessionServices;
        private readonly IOptionsSnapshot<BearerTokensOptions> OptionsSnapshot;
        public UnitOfWork(DatabaseContext _db,
                          IConfiguration _configuration,
                          IHostingEnvironment _hostEnvironment,
                          //IBackgroundJobClient _backgroundJobClient,
                          IMapper _mapper,
                           IOptionsSnapshot<BearerTokensOptions> _OptionsSnapshot,
                           ISessionServices _SessionServices,
                          HttpContextAccessor httpContextAccessor,
                           IMailServices _mailServices,
                           ISmsIntegrations _smsIntegrations,
                           HttpClient _httpClient)
        {
            db = _db;
            configuration = _configuration;
            //backgroundJobClient = _backgroundJobClient;
            contentRootPath = _hostEnvironment.ContentRootPath;
            webRootPath = _hostEnvironment.WebRootPath;
            mapper = _mapper;
            OptionsSnapshot = _OptionsSnapshot;
            SessionServices = _SessionServices;
            mailServices = _mailServices;
            smsIntegrations = _smsIntegrations;
            httpClient = _httpClient;
            httpContext = httpContextAccessor?.HttpContext;
            _HttpContextAccessor = httpContextAccessor;
        }

        IOptionsSnapshot<BearerTokensOptions> IUnitOfWork.OptionsSnapshot => OptionsSnapshot;
        public string ContentRootPath => this.contentRootPath;
        public string WebRootPath => this.webRootPath;
        public IBackgroundJobClient BackgroundJobClient => this.backgroundJobClient;
        public HttpContext HttpContext => this.httpContext;
        public HttpClient HttpClient => this.httpClient;
        public IHttpContextAccessor _httpContextAccessor => this._HttpContextAccessor;
        //public IHttpContextAccessor _httpContextAccessor => throw new NotImplementedException();
        public IMapper Mapper => this.mapper;
        public IConfiguration Configuration => this.configuration;
        ISessionServices IUnitOfWork.SessionServices => SessionServices;
        public IMailServices MailServices => mailServices;
        public ISmsIntegrations SmsIntegrations => smsIntegrations;

        private DatabaseContext dbContext;
        public DatabaseContext DbContext
        {
            get
            {
                if (this.dbContext == null) this.dbContext = db;
                return dbContext;
            }
        }
        private DatabaseFacade database;
        public DatabaseFacade Database
        {
            get
            {
                if (this.database == null) this.database = DbContext.Database;
                return database;
            }
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlRaw(sql, parameters);
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        private IMuslimeRepository muslime;
        public IMuslimeRepository Muslime
        {
            get
            {
                if (this.muslime == null)
                {
                    this.muslime = new MuslimeRepository(this);
                }
                return muslime;
            }
        }

        private IIslamRecognitionWayRepository islamRecognition;
        public IIslamRecognitionWayRepository IslamRecognition
        {
            get
            {
                if (this.islamRecognition == null)
                {
                    this.islamRecognition = new IslamRecognitionWayRepository(this);
                }
                return islamRecognition;
            }
        }

        private IWitnessRepository witness;
        public IWitnessRepository Witness
        {
            get
            {
                if (this.witness == null)
                {
                    this.witness = new WitnessRepository(this);
                }
                return witness;
            }
        }

        private ICommitteeRepository committee;
        public ICommitteeRepository Committee
        {
            get
            {
                if (this.committee == null)
                {
                    this.committee = new CommitteeRepository(this);
                }
                return committee;
            }
        }

        private IUserRepository user;
        public IUserRepository User
        {
            get
            {
                if (this.user == null)
                {
                    this.user = new UserRepository(this);
                }
                return user;
            }
        }

        private IUserTokenRepository userTokenRepository;
        public IUserTokenRepository UserTokenRepository
        {
            get
            {
                userTokenRepository ??= new UserTokenRepository(this);
                return userTokenRepository;
            }
        }

        private IIntegrationRepository integrationRepository;
        public IIntegrationRepository IntegrationRepository
        {
            get
            {
                integrationRepository ??= new IntegrationRepository(this);
                return integrationRepository;
            }
        }

        private ILocalizationRepository localizationRepository;
        public ILocalizationRepository LocalizationRepository
        {
            get
            {
                localizationRepository ??= new LocalizationRepository(this);
                return localizationRepository;
            }
        }


        private ITokenStoreRepository tokenStoreRepository;
        public ITokenStoreRepository TokenStoreRepository
        {
            get
            {
                tokenStoreRepository ??= new TokenStoreRepository(this);
                return tokenStoreRepository;
            }
        }

        private IDataProtectRepository dataProtectRepository;
        public IDataProtectRepository DataProtectRepository
        {
            get
            {
                dataProtectRepository ??= new DataProtectRepository();
                return dataProtectRepository;
            }
        }

        private ISecurityRepository securityRepository;
        public ISecurityRepository SecurityRepository
        {
            get
            {
                securityRepository ??= new SecurityRepository();
                return securityRepository;
            }
        }

        private IReportRepository report;
        public IReportRepository Report
        {
            get
            {
                if (this.report == null)
                {
                    this.report = new ReportRepository(this);
                }
                return report;
            }
        }





        //public async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
        //{
        //    byte[] BinaryContent = null;
        //    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
        //    {
        //        BinaryContent = binaryReader.ReadBytes((int)file.Length);
        //    }
        //    return BinaryContent;
        //}


        public async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file, int targetWidth, int targetHeight)
        {
            byte[] binaryContent = null;

            using (var memoryStream = new MemoryStream())
            {
                // Copy the file stream to a memory stream
                await file.CopyToAsync(memoryStream);

                // Resize the image
                using (var originalImage = Image.FromStream(memoryStream))
                {
                    using (var resizedImage = ResizeImage(originalImage, targetWidth, targetHeight))
                    {
                        // Convert the resized image to byte array
                        using (var resizedStream = new MemoryStream())
                        {
                            resizedImage.Save(resizedStream, originalImage.RawFormat);
                            binaryContent = resizedStream.ToArray();
                        }
                    }
                }
            }

            return binaryContent;
        }

        private static Image ResizeImage(Image originalImage, int targetWidth, int targetHeight)
        {
            int newWidth, newHeight;

            // Calculate new dimensions while maintaining the original aspect ratio
            if (originalImage.Width > originalImage.Height)
            {
                newWidth = targetWidth;
                newHeight = (int)((float)originalImage.Height / originalImage.Width * targetWidth);
            }
            else
            {
                newWidth = (int)((float)originalImage.Width / originalImage.Height * targetHeight);
                newHeight = targetHeight;
            }

            // Create a new bitmap with the specified dimensions
            var resizedImage = new Bitmap(newWidth, newHeight);

            // Draw the original image onto the new bitmap with the new dimensions
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }
    }
}
