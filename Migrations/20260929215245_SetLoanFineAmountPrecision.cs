using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UniversityLibraryMVC.Migrations
{
    /// <inheritdoc />
    public partial class SetLoanFineAmountPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Universities_UniversityID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Books_BookID",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Members_MemberID",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UniversityID",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "Universities",
                keyColumn: "UniversityID",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Members",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryColor",
                table: "Universities",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryColor",
                table: "Universities",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LogoPath",
                table: "Universities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Members",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MembershipType",
                table: "Members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Loans",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UniversityID",
                table: "Books",
                column: "UniversityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Universities_UniversityID",
                table: "Books",
                column: "UniversityID",
                principalTable: "Universities",
                principalColumn: "UniversityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Books_BookID",
                table: "Loans",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Members_MemberID",
                table: "Loans",
                column: "MemberID",
                principalTable: "Members",
                principalColumn: "MemberID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Universities_UniversityID",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Books_BookID",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Members_MemberID",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Books_UniversityID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Members",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryColor",
                table: "Universities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryColor",
                table: "Universities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "LogoPath",
                table: "Universities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Members",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Loans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "756fadba-0a4f-4dce-8bb5-4e170cb0e63e", "Admin", "ADMIN" },
                    { "2", "c79a99e3-d26d-4d9b-9279-a064e4e87a70", "Librarian", "LIBRARIAN" },
                    { "3", "4f4a4c18-ae31-4cec-8f5a-f982f70b279c", "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "Universities",
                columns: new[] { "UniversityID", "LogoPath", "MenuPosition", "Name", "PrimaryColor", "SecondaryColor" },
                values: new object[] { 1, "/images/meu-university-logo.png", 1, "Meu University", "#003366", "#FFD700" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UniversityID",
                table: "AspNetUsers",
                column: "UniversityID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Universities_UniversityID",
                table: "AspNetUsers",
                column: "UniversityID",
                principalTable: "Universities",
                principalColumn: "UniversityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Books_BookID",
                table: "Loans",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Members_MemberID",
                table: "Loans",
                column: "MemberID",
                principalTable: "Members",
                principalColumn: "MemberID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
