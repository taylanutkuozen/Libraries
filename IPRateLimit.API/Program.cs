using AspNetCoreRateLimit;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOptions();
//builder.Services.AddRateLimiter(options =>
//{
//    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
//    options.AddPolicy("fixed", HttpContext =>
//    {
//        return RateLimitPartition.GetFixedWindowLimiter(
//            partitionKey:HttpContext.Connection.RemoteIpAddress?.ToString(),
//            factory : _=> new FixedWindowRateLimiterOptions
//            {
//                PermitLimit=60,
//                Window=TimeSpan.FromSeconds(60),
//            }
//            );
//    });
//});
//builder.Services.AddRateLimiter(rateLimiterOptions =>
//{
//    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
//    {
//        options.PermitLimit = 10;
//        options.Window=TimeSpan.FromSeconds(10);
//        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 5;
//    });
//});
/*Key-Value �iftlerini bir class �zerinden okuma i�lemi ger�ekle�ebilecek. IP adreslerine izin verme, vermeme gibi durumlar� belirlemek i�in kullan�l�r*/
builder.Services.AddMemoryCache();/*Requestleri tutaca�� alan i�in kullan�l�r.*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
//AddSingleton uygulama run edildikten sonra bir kere y�klensin bir daha nesne �rne�i al�nmas�n. Bir tane instance run ediliyorsa uygulamada MemoryCache kullanmak uygun ama birden fazla instance al�n�yorsa uygulamadan merkezi bir memory de tutulmas� gerekmektedir. Redis gibi DistributedCache kullanmak gerekiyor. birden fazla olan instancelardaki request say�lar�n�n tutarl� olmas� a��s�ndan bu y�zden birden fazla instance i�in MemoryCacheIpPolicyStore yerine DistributedCacheIpPolicyStore kullan�m� gereklidir.
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//Ip adresi �zerinden datalar� tutacak counter'� belirledik.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
/*Request yapan�n IP adresini, header bilgisini okuyabilmesi i�in interface implement edildi.*/
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
/*limit rate �al��mas� i�in, IRateLimitConfiguration g�rd���n zaman RateLimitConfiguration instance'� al*/
//builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();
