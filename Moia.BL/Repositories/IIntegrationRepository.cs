using Microsoft.Reporting.Map.WebForms.BingMaps;
using Moia.Shared.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Moia.BL.Repositories
{
    public interface IIntegrationRepository
    {
        Task<string> GetToakenAsync(string language);
        Task<string> ManageCardAsync(ManageCardDto cardDto);
        Task<string> DeleteCardAsync(ManageCardDto cardDto);
    }

    public class IntegrationRepository : IIntegrationRepository
    {
        private readonly IUnitOfWork uow;



        public IntegrationRepository(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public async Task<string> GetToakenAsync(string language)
        {
            try
            {
                var requestBody = new
                {
                    clientId = EncryptHelper.Decrypt(uow.Configuration.GetSection("TawakkalnaIntegrationSettings:ClientId").Value),
                    clientSecret = EncryptHelper.Decrypt(uow.Configuration.GetSection("TawakkalnaIntegrationSettings:ClientSecret").Value),
                };

                var requestBodyJson = JsonSerializer.Serialize(requestBody);
                var request = new HttpRequestMessage(HttpMethod.Post, $"{uow.Configuration.GetSection("TawakkalnaIntegrationSettings:BaseUrl").Value}/{uow.Configuration.GetSection("TawakkalnaIntegrationSettings:LOGINURL").Value}");

                var Language = new StringWithQualityHeaderValue(language);
                request.Headers.AcceptLanguage.Add(Language);
                request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                var response = await uow.HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                    {
                        Message = "Tawakkalna Integration Login ",
                        Stacktrace = response.Content.ToString()
                    });
                    uow.SaveChanges();
                    return null;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                {
                    Message = "Tawakkalna Integration Login ",
                    Stacktrace = ex.StackTrace.ToString()
                });
                uow.SaveChanges();
                return null;
            }
        }

        public async Task<string> ManageCardAsync(ManageCardDto cardDto)
        {
            try
            {
                var baseUrl = uow.Configuration.GetSection("TawakkalnaIntegrationSettings:BaseUrl").Value;
                var apiUrl = uow.Configuration.GetSection("TawakkalnaIntegrationSettings:APIURL").Value;

                var requestUri = new Uri(new Uri(baseUrl), apiUrl);
                var queryString = $"?ActionType={cardDto.actionType}";

                requestUri = new Uri(requestUri, queryString);

                var requestBody = new
                {
                    referenceNo = cardDto.referenceNo,
                    payload = new
                    {
                        uniqueCardId = cardDto.uniqueCardId,
                        documentNo = cardDto.documentNo,
                        cardAttributes = cardDto.cardAttributes,
                        backCardAttributes = cardDto.backCardAttributes
                    }
                };

                var requestBodyJson = JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(cardDto.language));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cardDto.BearerToaken);
                request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                var response = await uow.HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                    {
                        Message = "Tawakkalna Integration ======>> Failed to manage card ",
                        Stacktrace = response.Content.ToString()
                    });
                    uow.SaveChanges();
                    return null;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                {
                    Message = "Tawakkalna Integration ======>> Failed to manage card ",
                    Stacktrace = ex.Message + "  StackTrace : " + ex.StackTrace,
                });
                uow.SaveChanges();
                return null;
            }
        }
        public async Task<string> DeleteCardAsync(ManageCardDto cardDto)
        {
            try
            {
                var baseUrl = uow.Configuration.GetSection("TawakkalnaIntegrationSettings:BaseUrl").Value;
                var apiUrl = uow.Configuration.GetSection("TawakkalnaIntegrationSettings:APIURL").Value;

                var requestUri = new Uri(new Uri(baseUrl), apiUrl);
                var queryString = $"?ActionType={cardDto.actionType}";

                requestUri = new Uri(requestUri, queryString);

                var requestBody = new
                {
                    referenceNo = cardDto.referenceNo,
                    payload = new
                    {
                        expirationDate = cardDto.expirationDate,
                        uniqueCardId = cardDto.uniqueCardId,
                        documentNo = cardDto.documentNo
                    }
                };

                var requestBodyJson = JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(cardDto.language));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cardDto.BearerToaken);
                request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                var response = await uow.HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                else
                {
                    uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                    {
                        Message = "Tawakkalna Integration ======>> Failed to manage card ",
                        Stacktrace = response.Content.ToString()
                    });
                    uow.SaveChanges();
                    return null;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                {
                    Message = "Tawakkalna Integration ======>> Failed to manage card ",
                    Stacktrace = ex.Message + "  StackTrace : " + ex.StackTrace,
                });
                uow.SaveChanges();
                return null;
            }
        }
    }

}



