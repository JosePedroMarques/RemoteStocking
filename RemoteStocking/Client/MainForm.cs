﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        ServerOps.ServerOpsClient proxy;
        public MainForm()
        {
            proxy = new ServerOps.ServerOpsClient();
            InitializeComponent();
            cbType.SelectedIndex = 0;
            cbShareType.Items.AddRange(proxy.GetAllSharesType());
            cbShareType.SelectedIndex = 0;
            cbCurrency.Items.AddRange(proxy.GetAllCurrency());
            cbCurrency.SelectedIndex = 0;
            
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtIDClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            if (txtPrice.Text != "" && txtEmail.Text != "" && txtIDClient.Text != "")
            {
                Stock.transactionType type = cbType.SelectedIndex == 1 ? type = Stock.transactionType.Sell : type = Stock.transactionType.Buy;
                Stock stock = new Stock(Stock.GenerateId(), Convert.ToInt32(txtIDClient.Text), txtEmail.Text, type, Convert.ToInt32(numQuantity.Value), Convert.ToString(cbShareType.SelectedItem.ToString()), DateTime.Now, Convert.ToDouble(txtPrice.Text), false, Convert.ToString(cbCurrency.SelectedItem.ToString()));
                MessageBox.Show(proxy.AddStock(stock), "Server response:", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("All fields must be filled!", "Field is empty!", MessageBoxButtons.OK);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtSearch.Text != "")
            {
                Stock[] stocks = proxy.GetAllStocksByClient(Convert.ToInt32(txtSearch.Text));
                foreach (Stock s in stocks)
                {
                    lbSearch.Items.Add(s.ToString());
                }
            }
            else
                MessageBox.Show("All fields must be filled!", "Field is empty!", MessageBoxButtons.OK);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.StopApplication();
        }

        

    }
}
