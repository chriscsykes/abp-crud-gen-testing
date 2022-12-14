using OneToManyTest.Hobbies;
using OneToManyTest.Orders;
using OneToManyTest.Customers;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.FileManagement.EntityFrameworkCore;

namespace OneToManyTest.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class OneToManyTestDbContext :
    AbpDbContext<OneToManyTestDbContext>,
    IIdentityProDbContext,
    ISaasDbContext
{
    public DbSet<Hobby> Hobbies { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public OneToManyTestDbContext(DbContextOptions<OneToManyTestDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentityPro();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureGdpr();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(OneToManyTestConsts.DbTablePrefix + "YourEntities", OneToManyTestConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Order>(b =>
{
    b.ToTable(OneToManyTestConsts.DbTablePrefix + "Orders", OneToManyTestConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Item).HasColumnName(nameof(Order.Item)).IsRequired();
    b.Property(x => x.Quantity).HasColumnName(nameof(Order.Quantity));
    b.Property(x => x.Price).HasColumnName(nameof(Order.Price));
});

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Customer>(b =>
{
    b.ToTable(OneToManyTestConsts.DbTablePrefix + "Customers", OneToManyTestConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.FirstName).HasColumnName(nameof(Customer.FirstName));
    b.Property(x => x.LastName).HasColumnName(nameof(Customer.LastName));
    b.Property(x => x.Email).HasColumnName(nameof(Customer.Email)).IsRequired();
    b.Property(x => x.Address).HasColumnName(nameof(Customer.Address));
    b.HasOne<Order>().WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
});

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Hobby>(b =>
{
    b.ToTable(OneToManyTestConsts.DbTablePrefix + "Hobbies", OneToManyTestConsts.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Name).HasColumnName(nameof(Hobby.Name)).IsRequired();
    b.Property(x => x.YearsPerformed).HasColumnName(nameof(Hobby.YearsPerformed));
    b.HasMany(x => x.Customers).WithOne().HasForeignKey(x => x.HobbyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
});

            builder.Entity<HobbyCustomer>(b =>
{
b.ToTable(OneToManyTestConsts.DbTablePrefix + "HobbyCustomer" + OneToManyTestConsts.DbSchema);
b.ConfigureByConvention();

b.HasKey(
    x => new { x.HobbyId, x.CustomerId }
);

b.HasOne<Hobby>().WithMany(x => x.Customers).HasForeignKey(x => x.HobbyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
b.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId).IsRequired().OnDelete(DeleteBehavior.NoAction);

b.HasIndex(
        x => new { x.HobbyId, x.CustomerId }
);
});
        }
        builder.ConfigureFileManagement();
        }
}