using Client.Services;

var builder = WebApplication.CreateBuilder (args);

builder.Services.AddRazorPages ();

builder.Services.AddHttpClient<ApiService> (client => {
    client.BaseAddress = new Uri ("http://localhost:5242/"); // Адрес API
});

var app = builder.Build ();

if (!app.Environment.IsDevelopment ()) {
    app.UseExceptionHandler ("/Error");
    app.UseHsts ();
}

app.UseHttpsRedirection ();

app.UseRouting ();

app.UseAuthorization ();

app.MapStaticAssets ();
app.MapRazorPages ()
   .WithStaticAssets ();

app.Run ();
