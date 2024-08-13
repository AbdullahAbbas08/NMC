using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moia.DAL.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CurrentResidence",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoorNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentResidence", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EducationalLevels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalLevels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Exceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stacktrace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exceptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyInformation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembersNumber = table.Column<int>(type: "int", nullable: true),
                    BoysNumber = table.Column<int>(type: "int", nullable: true),
                    GirlsNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyInformation", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IsslamRecognition",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsslamRecognition", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MainRoles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainRoles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MinistryBranshs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinistryBranshs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Preachers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preachers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ResidenceIssuePlace",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidenceIssuePlace", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Witness",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Witness", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Work",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalBox = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Work", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OriginalCountry",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryID = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoorNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalCountry", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OriginalCountry_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranshID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Department_MinistryBranshs_BranshID",
                        column: x => x.BranshID,
                        principalTable: "MinistryBranshs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameBeforeFristAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameBeforeMiddleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameBeforeLastAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAfter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameBeforeFristEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameBeforeMiddleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameBeforeLastEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IslamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NameAfterEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreacherNameID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonalData_Preachers_PreacherNameID",
                        column: x => x.PreacherNameID,
                        principalTable: "Preachers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalInformation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfEntryKingdom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalityID = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PreviousReligionID = table.Column<int>(type: "int", nullable: true),
                    PositionInFamily = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    HusbandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationalLevelID = table.Column<int>(type: "int", nullable: true),
                    ResidenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidenceIssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResidenceIssuePlaceID = table.Column<int>(type: "int", nullable: true),
                    PassportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfPassportIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlaceOfPassportIssue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalInformation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonalInformation_Countries_NationalityID",
                        column: x => x.NationalityID,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalInformation_EducationalLevels_EducationalLevelID",
                        column: x => x.EducationalLevelID,
                        principalTable: "EducationalLevels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalInformation_Religions_PreviousReligionID",
                        column: x => x.PreviousReligionID,
                        principalTable: "Religions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalInformation_ResidenceIssuePlace_ResidenceIssuePlaceID",
                        column: x => x.ResidenceIssuePlaceID,
                        principalTable: "ResidenceIssuePlace",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Committees",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Committees", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Committees_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDataWitness",
                columns: table => new
                {
                    PersonalDatasID = table.Column<int>(type: "int", nullable: false),
                    WitnessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDataWitness", x => new { x.PersonalDatasID, x.WitnessID });
                    table.ForeignKey(
                        name: "FK_PersonalDataWitness_PersonalData_PersonalDatasID",
                        column: x => x.PersonalDatasID,
                        principalTable: "PersonalData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalDataWitness_Witness_WitnessID",
                        column: x => x.WitnessID,
                        principalTable: "Witness",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Muslimes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalDataID = table.Column<int>(type: "int", nullable: true),
                    PersonalInformationID = table.Column<int>(type: "int", nullable: true),
                    OriginalCountryID = table.Column<int>(type: "int", nullable: true),
                    CurrentResidenceID = table.Column<int>(type: "int", nullable: true),
                    ContactDataID = table.Column<int>(type: "int", nullable: true),
                    FamilyInformationID = table.Column<int>(type: "int", nullable: true),
                    WorkID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muslimes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Muslimes_ContactData_ContactDataID",
                        column: x => x.ContactDataID,
                        principalTable: "ContactData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Muslimes_CurrentResidence_CurrentResidenceID",
                        column: x => x.CurrentResidenceID,
                        principalTable: "CurrentResidence",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Muslimes_FamilyInformation_FamilyInformationID",
                        column: x => x.FamilyInformationID,
                        principalTable: "FamilyInformation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Muslimes_OriginalCountry_OriginalCountryID",
                        column: x => x.OriginalCountryID,
                        principalTable: "OriginalCountry",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Muslimes_PersonalData_PersonalDataID",
                        column: x => x.PersonalDataID,
                        principalTable: "PersonalData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Muslimes_PersonalInformation_PersonalInformationID",
                        column: x => x.PersonalInformationID,
                        principalTable: "PersonalInformation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Muslimes_Work_WorkID",
                        column: x => x.WorkID,
                        principalTable: "Work",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageType = table.Column<int>(type: "int", nullable: false),
                    AttachmentValue = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    MuslimeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Attachment_Muslimes_MuslimeID",
                        column: x => x.MuslimeID,
                        principalTable: "Muslimes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IsslamRecognitionMuslime",
                columns: table => new
                {
                    IsslamRecognitionID = table.Column<int>(type: "int", nullable: false),
                    muslimesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsslamRecognitionMuslime", x => new { x.IsslamRecognitionID, x.muslimesID });
                    table.ForeignKey(
                        name: "FK_IsslamRecognitionMuslime_IsslamRecognition_IsslamRecognitionID",
                        column: x => x.IsslamRecognitionID,
                        principalTable: "IsslamRecognition",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IsslamRecognitionMuslime_Muslimes_muslimesID",
                        column: x => x.muslimesID,
                        principalTable: "Muslimes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MainUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentID = table.Column<int>(type: "int", nullable: true),
                    TryloginCount = table.Column<int>(type: "int", nullable: false),
                    ActiveDirectoryUser = table.Column<bool>(type: "bit", nullable: false),
                    Display = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MainUsers_Attachment_AttachmentID",
                        column: x => x.AttachmentID,
                        principalTable: "Attachment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainUsers_MinistryBranshs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MinistryBranshs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BranchNegoiationUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchNegoiationUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BranchNegoiationUsers_MainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchNegoiationUsers_MinistryBranshs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MinistryBranshs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentNegoiationUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentNegoiationUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DepartmentNegoiationUsers_Department_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentNegoiationUsers_MainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MainUserRole",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CommitteeId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    BranshId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainUserRole", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MainUserRole_Committees_CommitteeId",
                        column: x => x.CommitteeId,
                        principalTable: "Committees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainUserRole_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainUserRole_MainRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "MainRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainUserRole_MainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MainUserRole_MinistryBranshs_BranshId",
                        column: x => x.BranshId,
                        principalTable: "MinistryBranshs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEntryID = table.Column<int>(type: "int", nullable: true),
                    CommitteeID = table.Column<int>(type: "int", nullable: true),
                    MuslimeID = table.Column<int>(type: "int", nullable: true),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Orders_Committees_CommitteeID",
                        column: x => x.CommitteeID,
                        principalTable: "Committees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_MainUsers_DataEntryID",
                        column: x => x.DataEntryID,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Muslimes_MuslimeID",
                        column: x => x.MuslimeID,
                        principalTable: "Muslimes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessTokenExpiresDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RefreshTokenIdHash = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RefreshTokenIdHashSource = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RefreshTokenExpiresDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApplicationType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserToken_MainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MainUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_MuslimeID",
                table: "Attachment",
                column: "MuslimeID");

            migrationBuilder.CreateIndex(
                name: "IX_BranchNegoiationUsers_BranchId",
                table: "BranchNegoiationUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchNegoiationUsers_UserId",
                table: "BranchNegoiationUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Committees_DepartmentId",
                table: "Committees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_BranshID",
                table: "Department",
                column: "BranshID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentNegoiationUsers_BranchId",
                table: "DepartmentNegoiationUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentNegoiationUsers_UserId",
                table: "DepartmentNegoiationUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IsslamRecognitionMuslime_muslimesID",
                table: "IsslamRecognitionMuslime",
                column: "muslimesID");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_BranshId",
                table: "MainUserRole",
                column: "BranshId",
                unique: true,
                filter: "[BranshId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_CommitteeId",
                table: "MainUserRole",
                column: "CommitteeId");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_DepartmentId",
                table: "MainUserRole",
                column: "DepartmentId",
                unique: true,
                filter: "[DepartmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_RoleId",
                table: "MainUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MainUserRole_UserId",
                table: "MainUserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MainUsers_AttachmentID",
                table: "MainUsers",
                column: "AttachmentID");

            migrationBuilder.CreateIndex(
                name: "IX_MainUsers_BranchId",
                table: "MainUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_ContactDataID",
                table: "Muslimes",
                column: "ContactDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_CurrentResidenceID",
                table: "Muslimes",
                column: "CurrentResidenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_FamilyInformationID",
                table: "Muslimes",
                column: "FamilyInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_OriginalCountryID",
                table: "Muslimes",
                column: "OriginalCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_PersonalDataID",
                table: "Muslimes",
                column: "PersonalDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_PersonalInformationID",
                table: "Muslimes",
                column: "PersonalInformationID");

            migrationBuilder.CreateIndex(
                name: "IX_Muslimes_WorkID",
                table: "Muslimes",
                column: "WorkID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_OrderID",
                table: "OrderHistory",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CommitteeID",
                table: "Orders",
                column: "CommitteeID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DataEntryID",
                table: "Orders",
                column: "DataEntryID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MuslimeID",
                table: "Orders",
                column: "MuslimeID");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalCountry_CountryID",
                table: "OriginalCountry",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalData_PreacherNameID",
                table: "PersonalData",
                column: "PreacherNameID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalDataWitness_WitnessID",
                table: "PersonalDataWitness",
                column: "WitnessID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformation_EducationalLevelID",
                table: "PersonalInformation",
                column: "EducationalLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformation_NationalityID",
                table: "PersonalInformation",
                column: "NationalityID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformation_PreviousReligionID",
                table: "PersonalInformation",
                column: "PreviousReligionID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalInformation_ResidenceIssuePlaceID",
                table: "PersonalInformation",
                column: "ResidenceIssuePlaceID");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchNegoiationUsers");

            migrationBuilder.DropTable(
                name: "DepartmentNegoiationUsers");

            migrationBuilder.DropTable(
                name: "Exceptions");

            migrationBuilder.DropTable(
                name: "IsslamRecognitionMuslime");

            migrationBuilder.DropTable(
                name: "MainUserRole");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropTable(
                name: "PersonalDataWitness");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "IsslamRecognition");

            migrationBuilder.DropTable(
                name: "MainRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Witness");

            migrationBuilder.DropTable(
                name: "Committees");

            migrationBuilder.DropTable(
                name: "MainUsers");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "MinistryBranshs");

            migrationBuilder.DropTable(
                name: "Muslimes");

            migrationBuilder.DropTable(
                name: "ContactData");

            migrationBuilder.DropTable(
                name: "CurrentResidence");

            migrationBuilder.DropTable(
                name: "FamilyInformation");

            migrationBuilder.DropTable(
                name: "OriginalCountry");

            migrationBuilder.DropTable(
                name: "PersonalData");

            migrationBuilder.DropTable(
                name: "PersonalInformation");

            migrationBuilder.DropTable(
                name: "Work");

            migrationBuilder.DropTable(
                name: "Preachers");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "EducationalLevels");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "ResidenceIssuePlace");
        }
    }
}
