//// AnosheCms.Tests.Unit/Services/SubmissionServiceTests.cs
//// FULL REWRITE

//using AnosheCms.Application.DTOs.Form;
//using AnosheCms.Application.Interfaces;
//using AnosheCms.Domain.Entities;
//using AnosheCms.Infrastructure.Persistence.Data;
//using AnosheCms.Infrastructure.Services;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text; // (برای تست CSV)
//using System.Threading.Tasks;
//using Xunit;

//namespace AnosheCms.Tests.Unit.Services
//{
//    public class SubmissionServiceTests : IDisposable
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly SubmissionService _sut;
//        private readonly Guid _formId;
//        private const string FormSlug = "data-grid-form";

//        public SubmissionServiceTests()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            _context = new ApplicationDbContext(options, null);

//            _formId = Guid.NewGuid();

//            var form = new Form
//            {
//                Id = _formId,
//                ApiSlug = FormSlug,
//                Name = "Data Grid Form",
//                IsDeleted = false,
//                Fields = new List<FormField>
//                {
//                    // (ترتیب فیلدها عمداً نامرتب است تا تست OrderBy)
//                    new FormField { Name = "email", Label = "Email", FieldType = "Email", IsRequired = true, IsDeleted = false, Order = 2 },
//                    new FormField { Name = "full_name", Label = "Full Name", FieldType = "Text", IsRequired = true, IsDeleted = false, Order = 1 }
//                }
//            };
//            _context.Forms.Add(form);

//            // (افزودن دو پاسخ برای تست)
//            var submission1 = new FormSubmission { Id = Guid.NewGuid(), FormId = _formId, IpAddress = "1.1.1.1", CreatedDate = DateTime.UtcNow.AddDays(-1) };
//            submission1.SubmissionData = new List<FormSubmissionData>
//            {
//                new FormSubmissionData { FieldName = "full_name", FieldValue = "User A" },
//                new FormSubmissionData { FieldName = "email", FieldValue = "a@a.com" }
//            };
//            _context.FormSubmissions.Add(submission1);

//            var submission2 = new FormSubmission { Id = Guid.NewGuid(), FormId = _formId, IpAddress = "2.2.2.2", CreatedDate = DateTime.UtcNow };
//            submission2.SubmissionData = new List<FormSubmissionData>
//            {
//                new FormSubmissionData { FieldName = "full_name", FieldValue = "User B" }
//                // (پاسخ ناقص - فیلد ایمیل وجود ندارد)
//            };
//            _context.FormSubmissions.Add(submission2);

//            _context.SaveChanges();

//            _sut = new SubmissionService(_context);
//        }

//        // --- NEW TESTS (Grid & Export) ---

//        [Fact]
//        public async Task GetSubmissionGridAsync_ShouldReturnCorrectHeadersInOrder()
//        {
//            // Act
//            var result = await _sut.GetSubmissionGridAsync(_formId);

//            // Assert
//            Assert.NotNull(result.Headers);
//            Assert.Equal(4, result.Headers.Count); // (2 فیلد داینامیک + 2 فیلد ثابت)
//            // (تست OrderBy)
//            Assert.Equal("full_name", result.Headers[0].Name);
//            Assert.Equal("email", result.Headers[1].Name);
//            Assert.Equal("_SubmittedDate", result.Headers[2].Name);
//        }

//        [Fact]
//        public async Task GetSubmissionGridAsync_ShouldPivotDataCorrectly()
//        {
//            // Act
//            var result = await _sut.GetSubmissionGridAsync(_formId);

//            // Assert
//            Assert.Equal(2, result.Rows.Count); // (دو پاسخ)

//            // (تست ردیف دوم که جدیدتر است و باید اول بیاید)
//            var rowB = result.Rows[0];
//            Assert.Equal("User B", rowB["full_name"]);
//            Assert.Equal(string.Empty, rowB["email"]); // (تست مقدار خالی چون داده‌ای ارسال نشده)

//            // (تست ردیف اول که قدیمی‌تر است و باید دوم بیاید)
//            var rowA = result.Rows[1];
//            Assert.Equal("User A", rowA["full_name"]);
//            Assert.Equal("a@a.com", rowA["email"]);
//        }

//        [Fact]
//        public async Task ExportSubmissionsAsCsvAsync_ShouldContainHeadersAndData()
//        {
//            // Act
//            var csvBytes = await _sut.ExportSubmissionsAsCsvAsync(_formId);

//            // (حذف BOM برای تست محتوا)
//            var bom = Encoding.UTF8.GetPreamble();
//            var csvString = Encoding.UTF8.GetString(csvBytes.Skip(bom.Length).ToArray());

//            // Assert
//            Assert.NotNull(csvBytes);

//            var lines = csvString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

//            Assert.Equal(3, lines.Length); // (1 هدر + 2 ردیف)

//            // (تست هدر)
//            Assert.Equal("\"Full Name\",Email,تاریخ ارسال,IP", lines[0]);

//            // (تست ردیف اول - User B)
//            Assert.Contains("\"User B\",\"\",", lines[1]);

//            // (تست ردیف دوم - User A)
//            Assert.Contains("\"User A\",\"a@a.com\",", lines[2]);
//        }

//        // ... (تست‌های مربوط به SubmitFormAsync و ConditionalLogic از پکیج قبلی اینجا قرار می‌گیرند) ...

        
//        public void Dispose()
//        {
//            _context.Database.EnsureDeleted();
//            _context.Dispose();
//        }
//    }
//}