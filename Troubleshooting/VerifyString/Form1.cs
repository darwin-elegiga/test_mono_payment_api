using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VerifyString
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonGo1_Click(object sender, EventArgs e)
        {
            var serviceString = EmptyObjects();
            serviceString = ForBalanceRequest1(serviceString);

            string sampleText = serviceString.Pack();
            Clipboard.SetText(sampleText);
        }

        private StandardRequest EmptyObjects()
        {
            return new StandardRequest();
        }

        private StandardRequest ForBalanceRequest1(StandardRequest serviceString)
        {
            serviceString.CommonData.PassWord = "yraheem197";
            serviceString.CommonData.TransNumber = "55123182";
            serviceString.CommonData.User = "YAMMONRAHE";

            return serviceString;
        }

        private void buttonGo2_Click(object sender, EventArgs e)
        {
            string s1 = @"COMMONDATA            55123182                                                            0107Decrypt of card failed with error  ErrText: CRE0357                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 0000Successful Completion                                                                                                                                                                                                                                                                                                                                                                                                                         CARDDATA                                                                                                                                                                                                                                                                                                                                                                 CHECKDATA                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   CLAIMDATA                                                                                                                                                                                                                                                                                                                                                                                                                                                                  CORRSPDATA                                                                                                                                                                                                                                                                                                                                                                                                                                                                                COVERDITEM                                                                                                                                                                                                                                                                                                                           MERCHANT                                                                                                                     PAYMENT                                                       10.00                                                       10.00                                                                                                                                                                   SWITCHTRAN                                                                   0.0                                           0.0";
            //ServiceString serviceString = new ServiceString(false);
            //serviceString.PopulateDataFromISeriesResponse(s1);

            var response = TransactionWsStringHelpers.Unpack(s1);

            // CommonData commonData = new CommonData();
            // commonData.HydrateFromISeriesString(ref s1);
        }
    }
}
