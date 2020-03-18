using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lilian_Nishimaru_Sec003_COMP212_Lab04
{
    public partial class DisplayBooks : Form
    {
        public DisplayBooks()
        {
            InitializeComponent();
        }
        private void JoiningTableData_Load(object sender, EventArgs e)
        {
            // Entity Framework DbContext
            var dbcontext = new BookExamples.BooksEntities();

            //Store the last title to break the display
            String previousTitle = null;

            // 1) list of all the titles and the authors who wrote them. Sort the results by title.

            var authorsAndISBNs =
               from author in dbcontext.Authors
               from book in author.Titles
               orderby book.Title1
               select new { author.FirstName, author.LastName, book.Title1 };


            outputTextBox.AppendText("1) Titles and Authors:");

            //display authors and titles
            foreach (var element in authorsAndISBNs)
            {
                if (previousTitle == element.Title1)
                {
                    outputTextBox.AppendText($"\r\n\t\t\t " +
                   $"{element.FirstName} \t{element.LastName}");
                }
                else
                {
                    previousTitle = element.Title1;
                    outputTextBox.AppendText($"\r\n\t{element.Title1} " +
                   $"\r\n\t\t\t {element.FirstName} \t{element.LastName}");
                }
            }

            // 2) Get a list of all the titles and the authors who wrote them. Sort the results by title. Each title sort the authors
            //alphabetically by last name, then first name

            var authorsAndTitles2 =
                from author in dbcontext.Authors
                from book in author.Titles
                orderby book.Title1, author.LastName, author.FirstName
                select new { author.FirstName, author.LastName, book.Title1 };

            outputTextBox.AppendText("\r\n\r\n2) Titles and Authors:");      

            // display authors and titles in tabular format
            foreach (var element in authorsAndTitles2)
            {
                if (previousTitle == element.Title1)
                {
                    outputTextBox.AppendText($"\r\n\t\t\t" +
                   $"{element.FirstName} " + $" {element.LastName}");
                } else
                {
                    previousTitle = element.Title1;
                    outputTextBox.AppendText($"\r\n\t{element.Title1} " +
                  $"\r\n\t\t\t{element.FirstName} " + $" {element.LastName}");
                }
               
            }

            // Get a list of all the authors grouped by title, sorted by title; for a given title sort the author names
            //alphabetically by last name then first name

            var authorsGroupedByTitles =
                from book in dbcontext.Titles
                from author in book.Authors
                group author by new { book.Title1, author.LastName, author.FirstName } into r1
                orderby r1.Key, r1.Key.LastName, r1.Key.FirstName
                select new { r1.Key, r1.Key.FirstName, r1.Key.LastName };

            outputTextBox.AppendText($"\r\n 3) Authors Grouped by Titles ");
            previousTitle = null;

            foreach (var author in authorsGroupedByTitles)
            {
                if (previousTitle == author.Key.Title1)
                {
                    outputTextBox.AppendText($"\r\n\t\t\t {author.FirstName}, {author.LastName}");
                }
                else
                {
                    previousTitle = author.Key.Title1;
                    outputTextBox.AppendText($"\r\n\t" + $"{author.Key.Title1}");
                    outputTextBox.AppendText($"\r\n\t\t\t {author.FirstName}, {author.LastName}");
                }
            }
        }
    }
}
