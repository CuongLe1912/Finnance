using Microsoft.EntityFrameworkCore;

namespace URF.Core.EF.Trackable.NestedSets
{
    public static class NestedSetsModelBuilderExtensions
    {
        public static void ConfigureNestedSets<T>(this ModelBuilder modelBuilder)
            where T : class, INestedSet<T>
        {
            modelBuilder.Entity<T>()
                .HasOne(n => n.Parent)
                .WithMany(n => n.Childrens)
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
