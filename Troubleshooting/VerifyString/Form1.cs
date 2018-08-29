using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPay.Payment.Common.CommunicationStrings;
using VPay.Payment.Common.DataWebService;

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
            ServiceString serviceString = EmptyObjects();
            serviceString = ForBalanceRequest1(serviceString);

            string sampleText = serviceString.GenerateStringForISeriesCall();
            Clipboard.SetText(sampleText);
        }

        private ServiceString EmptyObjects()
        {
            return new ServiceString(new CommonData(), new CardData(), new CheckData(), new Claim(), new CorrespondenceData(),
                new CoveredItem(), new Merchant(), new Payment(), new SwitchTransaction());
        }

        private ServiceString ForBalanceRequest1(ServiceString serviceString)
        {
            serviceString.CommonSection.PassWord = "yraheem197";
            serviceString.CommonSection.TransNumber = "55123182";
            serviceString.CommonSection.User = "YAMMONRAHE";

            return serviceString;
        }

        private void buttonGo2_Click(object sender, EventArgs e)
        {
            string s1 = @"COMMONDATA            55123182                                                            0107Decrypt of card failed with error  ErrText: CRE0357                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 0000Successful Completion                                                                                                                                                                                                                                                                                                                                                                                                                         CARDDATA                                                                                                                                                                                                                                                                                                                                                                 CHECKDATA                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   CLAIMDATA                                                                                                                                                                                                                                                                                                                                                                                                                                                                  CORRSPDATA                                                                                                                                                                                                                                                                                                                                                                                                                                                                                COVERDITEM                                                                                                                                                                                                                                                                                                                           MERCHANT                                                                                                                     PAYMENT                                                       10.00                                                       10.00                                                                                                                                                                   SWITCHTRAN                                                                   0.0                                           0.0";
            ServiceString serviceString = new ServiceString();
            serviceString.PopulateDataFromISeriesResponse(s1);

            // CommonData commonData = new CommonData();
            // commonData.HydrateFromISeriesString(ref s1);
        }
    }
}
