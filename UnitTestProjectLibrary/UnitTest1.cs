using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Interface.Models;
using System.Windows.Controls;
using System.Linq;

namespace UnitTestProjectLibrary
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Author()
        {
            DiagramContainer db = new DiagramContainer();
            ListBox list = new ListBox();
            Interface.Classes.ViewData.UpdateTableAuthor(db, list);
            Assert.IsNotNull(list.Items);
        }

        [TestMethod]
        public void Reader()
        {
            DiagramContainer db = new DiagramContainer();
            DataGrid dgr = new DataGrid();
            Interface.Classes.ViewData.UpdateTableReader(db, dgr);
            Assert.IsNotNull(dgr.Items);
        }

        [TestMethod]
        public void TakeBooks()
        {
            DiagramContainer db = new DiagramContainer();
            DataGrid dgr = new DataGrid();
            Interface.Classes.ViewData.UpdateTableTakeBooks(db, dgr, 1);
            Assert.IsNotNull(dgr.Items);
        }

        [TestMethod]
        public void ComboArticul()
        {
            DiagramContainer db = new DiagramContainer();
            ComboBox cmb = new ComboBox();
            ListBox list = new ListBox();
            list.Items.Add("test");
            list.SelectedIndex = 0;
            Interface.Classes.ViewData.UpdateComboArticul(db, cmb, list);
            Assert.IsNotNull(cmb.Items);
        }

        [TestMethod]
        public void ListBook()
        {
            DiagramContainer db = new DiagramContainer();
            ListBox list = new ListBox();
            ListBox list2 = new ListBox();
            list2.Items.Add("test");
            list2.SelectedIndex = 0;
            Interface.Classes.ViewData.Update_listbook(db, list, list2);
            Assert.IsNotNull(list.Items);
        }

        [TestMethod]
        public void InsertData()
        {
            DiagramContainer db = new DiagramContainer();
            List<TextBox> list_tb = new List<TextBox>
            {
                new TextBox(),
                new TextBox(),
                new TextBox(),
                new TextBox()
            };
            Interface.Classes.ViewData.Insert_Data_Tb(db.ArticulBooks
                .FirstOrDefault(x => x.articul == 1), db, list_tb);
            Assert.IsNotNull(list_tb[0].Text);
        }
    }
}
