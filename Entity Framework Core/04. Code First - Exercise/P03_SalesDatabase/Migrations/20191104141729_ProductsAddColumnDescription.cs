using Microsoft.EntityFrameworkCore.Migrations;

namespace P03_SalesDatabase.Migrations
{
    public partial class ProductsAddColumnDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "Product",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Product",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                maxLength: 250,
                nullable: true,
                defaultValue: "No description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Product",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "decimal(5, 2)",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
