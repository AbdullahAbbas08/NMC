using Moia.Middlewares.Middlewares;
using Moia.Services;

namespace Moia.Middlewares
{
    public static class _Pipeline
    {
        public static void ConfigureHTTPRequestPipeline(this WebApplication app, Microsoft.Extensions.Hosting.IHostingEnvironment env, IConfiguration Configuration)
        {
            app.MapControllers();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else app.UseHsts();
            //app.UseResponseCompression();
            //app.UseSession();
            app.UseFileServer(new FileServerOptions() { EnableDirectoryBrowsing = false });

            app.UseCors(ServicesRegistration.CorsAllowedOriginsPolicyName);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            if (app.Environment.IsDevelopment())
            {
                app.Swagger();
                app.SinglePageApp(env, Configuration);
            }

        }
    }

}
