using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MyErp.Data.Context;

/// <summary>
/// PostgreSQL convention helpers. Postgres folds unquoted identifiers to lower
/// case, so tables and columns are mapped to snake_case rather than the CLR
/// PascalCase names.
/// </summary>
public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplySnakeCaseNames(this ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName is not null)
            {
                entity.SetTableName(ToSnakeCase(tableName));
            }

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            foreach (var key in entity.GetKeys())
            {
                var name = key.GetName();
                if (name is not null)
                {
                    key.SetName(ToSnakeCase(name));
                }
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var name = foreignKey.GetConstraintName();
                if (name is not null)
                {
                    foreignKey.SetConstraintName(ToSnakeCase(name));
                }
            }

            foreach (var index in entity.GetIndexes())
            {
                var name = index.GetDatabaseName();
                if (name is not null)
                {
                    index.SetDatabaseName(ToSnakeCase(name));
                }
            }
        }

        return modelBuilder;
    }

    private static string ToSnakeCase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var builder = new StringBuilder(value.Length + 8);

        for (var i = 0; i < value.Length; i++)
        {
            var current = value[i];

            if (char.IsUpper(current))
            {
                var previous = i > 0 ? value[i - 1] : '\0';
                var next = i + 1 < value.Length ? value[i + 1] : '\0';

                var startsNewWord = i > 0
                    && previous != '_'
                    && (!char.IsUpper(previous) || (char.IsUpper(previous) && char.IsLower(next)));

                if (startsNewWord)
                {
                    builder.Append('_');
                }

                builder.Append(char.ToLowerInvariant(current));
            }
            else
            {
                builder.Append(current);
            }
        }

        return builder.ToString();
    }
}
