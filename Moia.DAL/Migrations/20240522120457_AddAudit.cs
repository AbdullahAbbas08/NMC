using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moia.DAL.Migrations
{
    public partial class AddAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Work",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Work",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Work",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Work",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Witness",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Witness",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Witness",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Witness",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Witness",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Witness",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "UserToken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "UserToken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "UserToken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "UserToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Settings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Settings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Settings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ResidenceIssuePlace",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ResidenceIssuePlace",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "ResidenceIssuePlace",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ResidenceIssuePlace",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "ResidenceIssuePlace",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ResidenceIssuePlace",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Religions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Religions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Religions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Religions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Religions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Religions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Preachers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Preachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Preachers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Preachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Preachers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Preachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PersonalInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PersonalInformation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "PersonalInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PersonalInformation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PersonalInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PersonalInformation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PersonalData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PersonalData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "PersonalData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PersonalData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PersonalData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "PersonalData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "OriginalCountry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "OriginalCountry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "OriginalCountry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "OriginalCountry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "OriginalCountry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "OriginalCountry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "OrderHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "OrderHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "OrderHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "OrderHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "OrderHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "OrderHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Muslimes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Muslimes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Muslimes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Muslimes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Muslimes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Muslimes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MinistryBranshs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MinistryBranshs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "MinistryBranshs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "MinistryBranshs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "MinistryBranshs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MinistryBranshs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MainUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MainUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "MainUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "MainUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "MainUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MainUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MainUserRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MainUserRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "MainUserRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "MainUserRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "MainUserRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MainUserRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MainRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MainRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "MainRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "MainRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "MainRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MainRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Localizations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Localizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Localizations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Localizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Localizations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Localizations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "IsslamRecognition",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "IsslamRecognition",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "IsslamRecognition",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "IsslamRecognition",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "IsslamRecognition",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "IsslamRecognition",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "FamilyInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FamilyInformation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "FamilyInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FamilyInformation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "FamilyInformation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "FamilyInformation",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "EducationalLevels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "EducationalLevels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "EducationalLevels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "EducationalLevels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "EducationalLevels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "EducationalLevels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "DepartmentNegoiationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "DepartmentNegoiationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "DepartmentNegoiationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "DepartmentNegoiationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "DepartmentNegoiationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "DepartmentNegoiationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "CurrentResidence",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CurrentResidence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "CurrentResidence",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "CurrentResidence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "CurrentResidence",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "CurrentResidence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ContactData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ContactData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "ContactData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ContactData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "ContactData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ContactData",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Committees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Committees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Committees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Committees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Committees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Committees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "BranchNegoiationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "BranchNegoiationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "BranchNegoiationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "BranchNegoiationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "BranchNegoiationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "BranchNegoiationUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Attachment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Attachment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Attachment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Attachment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Attachment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Attachment",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Work");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Witness");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Witness");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Witness");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Witness");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Witness");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Witness");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "UserToken");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ResidenceIssuePlace");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ResidenceIssuePlace");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ResidenceIssuePlace");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ResidenceIssuePlace");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ResidenceIssuePlace");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ResidenceIssuePlace");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Religions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Preachers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Preachers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Preachers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Preachers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Preachers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Preachers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PersonalInformation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PersonalData");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PersonalData");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PersonalData");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PersonalData");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PersonalData");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "PersonalData");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OriginalCountry");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "OriginalCountry");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OriginalCountry");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "OriginalCountry");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OriginalCountry");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "OriginalCountry");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Muslimes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Muslimes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Muslimes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Muslimes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Muslimes");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Muslimes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MinistryBranshs");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MinistryBranshs");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "MinistryBranshs");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "MinistryBranshs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MinistryBranshs");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MinistryBranshs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MainUsers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MainUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "MainUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "MainUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MainUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MainUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MainUserRole");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MainRoles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MainRoles");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "MainRoles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "MainRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MainRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MainRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Localizations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "IsslamRecognition");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "IsslamRecognition");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "IsslamRecognition");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "IsslamRecognition");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "IsslamRecognition");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "IsslamRecognition");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FamilyInformation");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FamilyInformation");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "FamilyInformation");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FamilyInformation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FamilyInformation");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "FamilyInformation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "EducationalLevels");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "EducationalLevels");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "EducationalLevels");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "EducationalLevels");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "EducationalLevels");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "EducationalLevels");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DepartmentNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "DepartmentNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DepartmentNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "DepartmentNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DepartmentNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "DepartmentNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurrentResidence");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CurrentResidence");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CurrentResidence");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "CurrentResidence");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CurrentResidence");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "CurrentResidence");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ContactData");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ContactData");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ContactData");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ContactData");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ContactData");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ContactData");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Committees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BranchNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "BranchNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BranchNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "BranchNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "BranchNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "BranchNegoiationUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Attachment");
        }
    }
}
