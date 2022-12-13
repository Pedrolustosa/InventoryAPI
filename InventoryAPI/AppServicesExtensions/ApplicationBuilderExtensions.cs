namespace InventoryAPI.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(prop =>
            {
                prop.AllowAnyOrigin();
                prop.AllowAnyMethod();
                prop.WithMethods("GET");
            });

            return app; 
        }

        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { });
            return app;
        }
    }
}
