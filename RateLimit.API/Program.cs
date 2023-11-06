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
/*Key-Value çiftlerini bir class üzerinden okuma iþlemi gerçekleþebilecek. IP adreslerine izin verme, vermeme gibi durumlarý belirlemek için kullanýlýr*/
builder.Services.AddMemoryCache();/*Requestleri tutacaðý alan için kullanýlýr.*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IpRateLimitOptions>((IConfiguration)config.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>((IConfiguration)config.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>(); 
//AddSingleton uygulama run edildikten sonra bir kere yüklensin bir daha nesne örneði alýnmasýn. Bir tane instance run ediliyorsa uygulamada MemoryCache kullanmak uygun ama birden fazla instance alýnýyorsa uygulamadan merkezi bir memory de tutulmasý gerekmektedir. Redis gibi DistributedCache kullanmak gerekiyor. birden fazla olan instancelardaki request sayýlarýnýn tutarlý olmasý açýsýndan bu yüzden birden fazla instance için MemoryCacheIpPolicyStore yerine DistributedCacheIpPolicyStore kullanýmý gereklidir.
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
//Ip adresi üzerinden datalarý tutacak counter'ý belirledik.
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
