using Microsoft.EntityFrameworkCore;
using System.Xml;
using UniversityLibraryMVC.Models;

namespace UniversityLibraryMVC.Services
{
    public class XmlService
    {
        private readonly LibraryDbContext _context;

        public XmlService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<string> ExportBooksToXmlAsync()
        {
            var books = await _context.Books.ToListAsync(); 

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Books");
            xmlDoc.AppendChild(root);

            foreach (var book in books)
            {
                var bookElement = xmlDoc.CreateElement("Book");

                var isbn = xmlDoc.CreateElement("ISBN");
                isbn.InnerText = book.ISBN;
                bookElement.AppendChild(isbn);

                var title = xmlDoc.CreateElement("Title");
                title.InnerText = book.Title;
                bookElement.AppendChild(title);

                var author = xmlDoc.CreateElement("Author");
                author.InnerText = book.Author;
                bookElement.AppendChild(author);

                var publisher = xmlDoc.CreateElement("Publisher");
                publisher.InnerText = book.Publisher;
                bookElement.AppendChild(publisher);

                var publicationYear = xmlDoc.CreateElement("PublicationYear");
                publicationYear.InnerText = book.PublicationYear?.ToString() ?? "";
                bookElement.AppendChild(publicationYear);

                var category = xmlDoc.CreateElement("Category");
                category.InnerText = book.Category;
                bookElement.AppendChild(category);

                var totalCopies = xmlDoc.CreateElement("TotalCopies");
                totalCopies.InnerText = book.TotalCopies.ToString();
                bookElement.AppendChild(totalCopies);

                var availableCopies = xmlDoc.CreateElement("AvailableCopies");
                availableCopies.InnerText = book.AvailableCopies.ToString();
                bookElement.AppendChild(availableCopies);

                root.AppendChild(bookElement);
            }

            return xmlDoc.OuterXml;
        }

        public async Task<string> ExportMembersToXmlAsync()
        {
            var members = await _context.Members.ToListAsync();

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Members");
            xmlDoc.AppendChild(root);

            foreach (var member in members)
            {
                var memberElement = xmlDoc.CreateElement("Member");

                var studentId = xmlDoc.CreateElement("StudentID");
                studentId.InnerText = member.StudentID;
                memberElement.AppendChild(studentId);

                var firstName = xmlDoc.CreateElement("FirstName");
                firstName.InnerText = member.FirstName;
                memberElement.AppendChild(firstName);

                var lastName = xmlDoc.CreateElement("LastName");
                lastName.InnerText = member.LastName;
                memberElement.AppendChild(lastName);

                var email = xmlDoc.CreateElement("Email");
                email.InnerText = member.Email;
                memberElement.AppendChild(email);

                var phone = xmlDoc.CreateElement("Phone");
                phone.InnerText = member.Phone;
                memberElement.AppendChild(phone);

                var address = xmlDoc.CreateElement("Address");
                address.InnerText = member.Address;
                memberElement.AppendChild(address);

                var age = xmlDoc.CreateElement("Age");
                age.InnerText = member.Age?.ToString() ?? "";
                memberElement.AppendChild(age);

                var membershipType = xmlDoc.CreateElement("MembershipType");
                membershipType.InnerText = member.MembershipType;
                memberElement.AppendChild(membershipType);

                var registrationDate = xmlDoc.CreateElement("RegistrationDate");
                registrationDate.InnerText = member.RegistrationDate.ToString("yyyy-MM-dd");
                memberElement.AppendChild(registrationDate);

                var isActive = xmlDoc.CreateElement("IsActive");
                isActive.InnerText = member.IsActive.ToString();
                memberElement.AppendChild(isActive);

                root.AppendChild(memberElement);
            }

            return xmlDoc.OuterXml;
        }

        public async Task<string> ExportLoansToXmlAsync()
        {
            var loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync(); 

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Loans");
            xmlDoc.AppendChild(root);

            foreach (var loan in loans)
            {
                var loanElement = xmlDoc.CreateElement("Loan");

                var bookTitle = xmlDoc.CreateElement("BookTitle");
                bookTitle.InnerText = loan.Book?.Title ?? "";
                loanElement.AppendChild(bookTitle);

                var memberName = xmlDoc.CreateElement("MemberName");
                memberName.InnerText = loan.Member?.Name ?? "";
                loanElement.AppendChild(memberName);

                var loanDate = xmlDoc.CreateElement("LoanDate");
                loanDate.InnerText = loan.LoanDate.ToString("yyyy-MM-dd");
                loanElement.AppendChild(loanDate);

                var dueDate = xmlDoc.CreateElement("DueDate");
                dueDate.InnerText = loan.DueDate.ToString("yyyy-MM-dd");
                loanElement.AppendChild(dueDate);

                var returnDate = xmlDoc.CreateElement("ReturnDate");
                returnDate.InnerText = loan.ReturnDate?.ToString("yyyy-MM-dd") ?? "";
                loanElement.AppendChild(returnDate);

                var status = xmlDoc.CreateElement("Status");
                status.InnerText = loan.Status;
                loanElement.AppendChild(status);

                var fineAmount = xmlDoc.CreateElement("FineAmount");
                fineAmount.InnerText = loan.FineAmount.ToString("F2");
                loanElement.AppendChild(fineAmount);

                root.AppendChild(loanElement);
            }

            return xmlDoc.OuterXml;
        }

        public async Task ImportBooksFromXmlAsync(string xmlContent)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            var books = xmlDoc.SelectNodes("//Book");
            if (books != null)
            {
                foreach (XmlNode bookNode in books)
                {
                    var book = new Book
                    {
                        ISBN = GetNodeValue(bookNode, "ISBN"),
                        Title = GetNodeValue(bookNode, "Title"),
                        Author = GetNodeValue(bookNode, "Author"),
                        Publisher = GetNodeValue(bookNode, "Publisher"),
                        Category = GetNodeValue(bookNode, "Category"),
                        TotalCopies = int.Parse(GetNodeValue(bookNode, "TotalCopies", "1")),
                        AvailableCopies = int.Parse(GetNodeValue(bookNode, "AvailableCopies", "1")),
                        UniversityID = 1
                    };

                    if (int.TryParse(GetNodeValue(bookNode, "PublicationYear"), out int pubYear))
                    {
                        book.PublicationYear = pubYear;
                    }

                    _context.Books.Add(book);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task ImportMembersFromXmlAsync(string xmlContent)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            var members = xmlDoc.SelectNodes("//Member");
            if (members != null)
            {
                foreach (XmlNode memberNode in members)
                {
                    var member = new Member
                    {
                        StudentID = GetNodeValue(memberNode, "StudentID"),
                        FirstName = GetNodeValue(memberNode, "FirstName"),
                        LastName = GetNodeValue(memberNode, "LastName"),
                        Email = GetNodeValue(memberNode, "Email"),
                        Phone = GetNodeValue(memberNode, "Phone"),
                        Address = GetNodeValue(memberNode, "Address"),
                        MembershipType = GetNodeValue(memberNode, "MembershipType", "Student"),
                        RegistrationDate = DateTime.Parse(GetNodeValue(memberNode, "RegistrationDate", DateTime.Now.ToString("yyyy-MM-dd"))),
                        IsActive = bool.Parse(GetNodeValue(memberNode, "IsActive", "true")),
                        UniversityID = 1
                    };

                    if (int.TryParse(GetNodeValue(memberNode, "Age"), out int age))
                    {
                        member.Age = age;
                    }

                    _context.Members.Add(member);
                }
                await _context.SaveChangesAsync();
            }
        }

        private string GetNodeValue(XmlNode parent, string nodeName, string defaultValue = "")
        {
            var node = parent.SelectSingleNode(nodeName);
            return node?.InnerText ?? defaultValue;
        }
    }
}