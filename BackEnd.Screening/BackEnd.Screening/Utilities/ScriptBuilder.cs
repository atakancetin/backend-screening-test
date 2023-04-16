using BackEnd.Screening.Common;
using System.Text;

namespace BackEnd.Screening.Utilities
{
    public class ScriptBuilder
    {
        private static StringBuilder _builder = new StringBuilder();

        public string CheckLogin()
        {
            ResetBuilder();
            _builder.Append("(() => {");
            _builder.Append(ScriptConstants.resultScript).ToString();
            _builder.Append("})();");
            return _builder.ToString();
        }
        public string GenerateLoginScript(string email, string password)
        {
            ResetBuilder();
            _builder.Append("(async() => {");
            _builder.Append(ScriptConstants.menuClick);
            GenerateWaitScript(1000);
            _builder.Append(ScriptConstants.signInClick);
            GenerateWaitScript(750);
            _builder.Append(string.Format(ScriptConstants.email, email));
            GenerateWaitScript();
            _builder.Append(string.Format(ScriptConstants.password, password));
            GenerateWaitScript(750);
            _builder.Append(ScriptConstants.loginClick);
            _builder.Append("})();");
            return _builder.ToString();
        }
        public string GenerateFilterString(string stockType, string make, string model, int maxPrice, string distance, int zip)
        {
            ResetBuilder();
            _builder.Append(string.Format(ScriptConstants.stockTypeValueChange, stockType));
            _builder.Append(string.Format(ScriptConstants.makeTypeValueChange, make));
            _builder.Append(string.Format(ScriptConstants.modelTypeValueChange, model));
            _builder.Append(string.Format(ScriptConstants.maxPriceValueChange, maxPrice));
            _builder.Append(string.Format(ScriptConstants.maxDistanceValueChange, distance));
            _builder.Append(string.Format(ScriptConstants.zipChange, zip));
            _builder.Append(ScriptConstants.searchButtonClick);
            return _builder.ToString();
        }
        public void GenerateWaitScript(int ms = 500)
        {
            _builder.Append(string.Format(ScriptConstants.waitNSecond, ms));
        }

        public string GenerateSearchResultsScript()
        {
            ResetBuilder();
            return _builder.Append(ScriptConstants.GatherSearchResultScript).ToString();
        }
        public string GenerateGoToSecondPageScript()
        {
            ResetBuilder();
            _builder.Append(string.Format(ScriptConstants.goToNPage, 2));
            return _builder.ToString();
        }
        public string GenerateSelectCarScript()
        {
            ResetBuilder();
            _builder.Append(ScriptConstants.selectModelYCar);
            return _builder.ToString();
        }
        public string GenerateGatherCarDetailScript()
        {
            ResetBuilder();
            return _builder.Append(ScriptConstants.GatherCarDetailScript).ToString();
        }
        public string ClickHomeDeliveryButton()
        {
            ResetBuilder();
            _builder.Append(ScriptConstants.clickHomeDelivery);
            return _builder.ToString();
        }
        public string GatherHomeToDeliveryData()
        {
            ResetBuilder();
            return _builder.Append(ScriptConstants.GatherHomeDeliveryData).ToString();
        }
        public string GenerateSelectModelXScript()
        {
            ResetBuilder();
            _builder.Append(ScriptConstants.selectModelX);
            return _builder.ToString();
        }
        private void ResetBuilder()
        {
            if (_builder.Length > 0)
            {
                _builder.Clear();
            }
        }
    }
}
