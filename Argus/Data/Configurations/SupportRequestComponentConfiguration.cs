using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Argus.Models;

namespace Argus.Data.Configurations
{
    public class SupportRequestComponentConfiguration : IEntityTypeConfiguration<SupportRequestComponent>
    {
        public void Configure(EntityTypeBuilder<SupportRequestComponent> builder)
        {
            // 1. Имя таблицы
            //builder.ToTable("SupportRequestComponents");

            // 2. РЕШЕНИЕ ОШИБКИ: Задаем составной первичный ключ
            // EF Core поймет, что уникальность строки определяется связкой Заявка + Деталь
            builder.HasKey(src => new { src.SupportRequestId, src.ComponentId });

            // 3. Защищаем поле Quantity от отрицательных значений на уровне БД (Check Constraint)
            builder.Property(src => src.Quantity)
                .IsRequired();

            builder.ToTable(t => t.HasCheckConstraint("CK_Quantity_Positive", "quantity > 0"));

            // 4. Явная настройка связей (Foreign Keys)
            // Связь с заявкой
            builder.HasOne(src => src.SupportRequest)
                .WithMany() // У заявки может быть много потраченных деталей
                .HasForeignKey(src => src.SupportRequestId)
                .OnDelete(DeleteBehavior.Cascade); // Если удаляем заявку, удаляются и записи о ее деталях

            // Связь с деталью (складом)
            builder.HasOne(src => src.Component)
                .WithMany()
                .HasForeignKey(src => src.ComponentId)
                .OnDelete(DeleteBehavior.Restrict); // ЗАПРЕЩАЕМ удалять деталь со склада, если она привязана к истории ремонта
        }
    }
}