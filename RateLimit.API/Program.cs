using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
System.Configuration.Configuration config =
               System.Configuration.ConfigurationManager.OpenExeConfiguration(
               ConfigurationUserLevel.None);
builder.Services.AddControllers();
builder.Services.AddOptions();
/*Key-Value �iftlerini bir class �zerinden okuma i�lemi ger�ekle�ebilecek. IP adreslerine izin verme, vermeme gibi durumlar� belirlemek i�in kullan�l�r*/
builder.Services.AddMemoryCache();/*Requestleri tutaca�� alan i�in kullan�l�r.*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IpRateLimitOptions>((IConfiguration)config.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>((IConfiguration)config.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>(); 
//AddSingleton uygulama run edildikten sonra bir kere y�klensin bir daha nesne �rne�i al�nmas�n. Bir tane instance run ediliyorsa uygulamada MemoryCache kullanmak uygun ama birden fazla instance al�n�yorsa uygulamadan merkezi bir memory de tutulmas� gerekmektedir. Redis gibi DistributedCache kullanmak gerekiyor. birden fazla olan instancelardaki request say�lar�n�n tutarl� olmas� a��s�ndan bu y�zden birden fazla instance i�in MemoryCacheIpPolicyStore yerine DistributedCacheIpPolicyStore kullan�m� gereklidir.
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//Ip adresi �zerinden datalar� tutacak counter'� belirledik.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
