namespace LearningSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ExamTableExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ExamSubmission",
                table: "Exams",
                maxLength: 2097152,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldMaxLength: 2097152,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExamFileName",
                table: "Exams",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamFileName",
                table: "Exams");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ExamSubmission",
                table: "Exams",
                maxLength: 2097152,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 2097152);
        }
    }
}
