namespace LearningSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ResizeExamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ExamSubmission",
                table: "Exams",
                maxLength: 2097152,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 2048,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ExamSubmission",
                table: "Exams",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 2097152,
                oldNullable: true);
        }
    }
}
