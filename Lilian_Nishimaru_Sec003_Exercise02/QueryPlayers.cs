using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lilian_Nishimaru_Sec003_Exercise02
{
    public partial class QueryPlayers : Form
    {
        public QueryPlayers()
        {
            InitializeComponent();
        }

        //Entity Framework DbContext
        private BaseballData.BaseballEntities dbContext = new BaseballData.BaseballEntities();  

        //Load data from database into the query GridView
        private void PlayerQueries_load(object sender, EventArgs e)
        {
            // load Player's Table ordered by LastName then FirstName
            dbContext.Players
                .OrderBy(player => player.LastName)
                .ThenBy(player => player.FirstName)
                .Load();

            //Specify DataSource for playerBindingSource
            playerBindingSource.DataSource = dbContext.Players.Local;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            // load Player's found by LastName

            playerBindingSource.DataSource = dbContext.Players.Local
                .Where(player => player.LastName.ToLower() == lastNameTextBox.Text.ToLower())
                .OrderBy(player => player.FirstName);

                playerBindingSource.MoveFirst();

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            lastNameTextBox.Text = null;
            PlayerQueries_load(this, null);
        }

        private void playerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            Validate();
            playerBindingSource.EndEdit();
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                MessageBox.Show("Error Saving");
            }
        }
    }
}
