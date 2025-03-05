using Client.Models;
using Client.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder (args);

builder.Services.AddRazorPages ();

// Регистрируем HttpClient для ApiService
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri ("http://localhost:5242/"); // Адрес API
});

// Добавляем кэш для сессий
builder.Services.AddDistributedMemoryCache (); // Это необходимо для хранения данных сессии в памяти

// Настройка сессий
builder.Services.AddSession (options => {
    options.IdleTimeout = TimeSpan.FromMinutes (30); // Время жизни сессии
    options.Cookie.HttpOnly = true; // Защищаем сессию
    options.Cookie.IsEssential = true; // Сессия будет необходимой для приложения
});

// Регистрируем IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();
builder.Services.AddSingleton<UserModel> ();



WebApplication app = builder.Build ();




if (!app.Environment.IsDevelopment ()) {
    app.UseExceptionHandler ("/Error");
    app.UseHsts ();
}

app.UseHttpsRedirection ();
app.UseStaticFiles ();

// Используем сессии
app.UseSession ();
app.UseRouting ();
app.UseAuthentication ();
app.UseAuthorization ();

// Карты маршрутов
app.MapRazorPages ().WithStaticAssets ();




app.Run ();



