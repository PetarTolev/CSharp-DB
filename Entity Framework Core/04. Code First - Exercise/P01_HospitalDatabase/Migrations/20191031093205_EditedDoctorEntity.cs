using Microsoft.EntityFrameworkCore.Migrations;

namespace P01_HospitalDatabase.Migrations
{
    public partial class EditedDoctorEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Doctors_DoctorId1",
                table: "Visitations");

            migrationBuilder.DropIndex(
                name: "IX_Visitations_DoctorId1",
                table: "Visitations");

            migrationBuilder.DropColumn(
                name: "DoctorId1",
                table: "Visitations");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Visitations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitations_DoctorId",
                table: "Visitations",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Doctors_DoctorId",
                table: "Visitations",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitations_Doctors_DoctorId",
                table: "Visitations");

            migrationBuilder.DropIndex(
                name: "IX_Visitations_DoctorId",
                table: "Visitations");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Visitations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DoctorId1",
                table: "Visitations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitations_DoctorId1",
                table: "Visitations",
                column: "DoctorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitations_Doctors_DoctorId1",
                table: "Visitations",
                column: "DoctorId1",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
