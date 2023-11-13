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
/*Key-Value çiftlerini bir class üzerinden okuma iþlemi gerçekleþebilecek. IP adreslerine izin verme, vermeme gibi durumlarý belirlemek için kullanýlýr*/
builder.Services.AddMemoryCache();/*Requestleri tutacaðý alan için kullanýlýr.*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
//AddSingleton uygulama run edildikten sonra bir kere yüklensin bir daha nesne örneði alýnmasýn. Bir tane instance run ediliyorsa uygulamada MemoryCache kullanmak uygun ama birden fazla instance alýnýyorsa uygulamadan merkezi bir memory de tutulmasý gerekmektedir. Redis gibi DistributedCache kullanmak gerekiyor. birden fazla olan instancelardaki request sayýlarýnýn tutarlý olmasý açýsýndan bu yüzden birden fazla instance için MemoryCacheIpPolicyStore yerine DistributedCacheIpPolicyStore kullanýmý gereklidir.
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//Ip adresi üzerinden datalarý tutacak counter'ý belirledik.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
/*Request yapanýn IP adresini, header bilgisini okuyabilmesi için interface implement edildi.*/
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
/*limit rate çalýþmasý için, IRateLimitConfiguration gördüðün zaman RateLimitConfiguration instance'ý al*/
//builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.Run();
