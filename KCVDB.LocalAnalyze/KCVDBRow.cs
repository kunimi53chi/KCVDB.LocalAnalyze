using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze
{
    /// <summary>
    /// KCVDBログの行あたりのアイテム
    /// </summary>
    public class KCVDBRow
    {
        /// <summary>
        /// セッションIDを表すUUID文字列
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 送信クライアントとそのバージョンを表す文字列
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// 艦これAPIの絶対URL。スキーマ(http)、ホスト(IPアドレスまたはドメイン)を含む
        /// </summary>
        public string RequestUri
        {
            get { return _requestUri; }
            set { _requestUri = value; _kcsapiPaths = null; }
        }
        /// <summary>
        /// 艦これAPIのレスポンスのステータスコードを表す文字列
        /// </summary>
        public string StatusCodeString { get; set; }
        /// <summary>
        /// 艦これAPIのレスポンスヘッダーのDateフィールドから得られる文字列
        /// </summary>
        public string HttpDateString
        {
            get { return _httpDateString; }
            set { _httpDateString = value; _httpDate = null; }
        }
        /// <summary>
        /// 送信クライアントが艦これAPIを受信した日時を表すRFC1123形式の文字列
        /// </summary>
        public string LocalTimeString
        {
            get { return _localTimeString; }
            set { _localTimeString = value; _localTime = null; }
        }
        /// <summary>
        /// 艦これAPIのリクエストボディ
        /// </summary>
        public string RequestValue { get; set; }
        /// <summary>
        /// 艦これAPIのレスポンスボディ。svdata=等のプレフィックスも含む
        /// </summary>
        public string ResponseValue { get; set; }

        #region Stringをパースしたプロパティ
        int _statusCode = -1;
        /// <summary>
        /// 艦これAPIのレスポンスのステータスコードを表す数値
        /// </summary>
        public int StatusCode
        {
            get
            {
                if(_statusCode == -1)
                {
                    int.TryParse(StatusCodeString, out _statusCode);
                }
                return (int)_statusCode;
            }
        }

        string _httpDateString;
        DateTimeOffset? _httpDate = null;
        /// <summary>
        /// 艦これAPIのレスポンスヘッダーのDateフィールドから得られる日時
        /// </summary>
        public DateTimeOffset HttpDate
        {
            get
            {
                if(_httpDate == null)
                {
                    DateTimeOffset dto;
                    DateTimeHelper.TryParseRFC1123(_httpDateString, out dto);
                    _httpDate = dto;
                }
                return (DateTimeOffset)_httpDate;
            }
        }

        string _localTimeString;
        DateTimeOffset? _localTime = null;
        /// <summary>
        /// 送信クライアントが艦これAPIを受信した日時
        /// </summary>
        public DateTimeOffset LocalTime
        {
            get
            {
                if(_localTime == null)
                {
                    DateTimeOffset dto;
                    DateTimeHelper.TryParseRFC1123(_localTimeString, out dto);
                    _localTime = dto;
                }
                return (DateTimeOffset)_localTime;
            }
        }

        string[] _kcsapiPaths = null;
        string _requestUri;
        /// <summary>
        /// 艦これAPIの親アドレス（api_port, api_get_member等）
        /// RequestUriがnullの場合はnull, それ以外で取得に失敗した場合はstring.Emptyを返します
        /// </summary>
        public string KcsapiParent
        {
            get
            {
                if (_requestUri == null) return null;
                if(_kcsapiPaths == null)
                {
                    _kcsapiPaths = _requestUri.Replace("//", "/").Split('/');
                }
                if (_kcsapiPaths.Length < 4) return string.Empty;
                return _kcsapiPaths[3];
            }
        }

        /// <summary>
        /// 艦これAPIの子アドレス（slotitem, port, battle等）
        /// RequestUriがnullの場合はnull, それ以外で取得に失敗した場合はstring.Emptyを返します
        /// </summary>
        public string KcsapiChildren
        {
            get
            {
                if (_requestUri == null) return null;
                if (_kcsapiPaths == null)
                {
                    _kcsapiPaths = _requestUri.Replace("//", "/").Split('/');
                }
                if (_kcsapiPaths.Length < 5) return string.Empty;
                return _kcsapiPaths[4];
            }
        }
        #endregion

        /// <summary>
        /// KCVDBのログの行単位の文字列のパースを試みます。戻り値は変換に成功したかどうかを示します
        /// </summary>
        /// <param name="lineStr">KCVDBの行単位の文字列</param>
        /// <param name="kcvdbRow">変換に成功した場合は対応するKCVDBRow、失敗した場合はnew KCVDBRow()が格納されます</param>
        /// <returns></returns>
        public static bool TryParse(string lineStr, out KCVDBRow kcvdbRow)
        {
            if (lineStr == null)
            {
                kcvdbRow = new KCVDBRow();
                return false;
            }

            var cell = lineStr.Split('\t');
            // [0] AgentID
            // [1] SessionID
            // [2] Path
            // [3] HttpStatusCode
            // [4] ServerTime
            // [5] LocalTime
            // [6] RequestBody
            // [7] ResponseBody

            if(cell.Length < 8)
            {
                kcvdbRow = new KCVDBRow();
                return false;
            }

            kcvdbRow = new KCVDBRow();
            kcvdbRow.AgentId = cell[0];
            kcvdbRow.SessionId = cell[1];
            kcvdbRow.RequestUri = cell[2];
            kcvdbRow.StatusCodeString = cell[3];
            kcvdbRow.HttpDateString = cell[4];
            kcvdbRow.LocalTimeString = cell[5];
            kcvdbRow.RequestValue = cell[6];
            kcvdbRow.ResponseValue = cell[7];

            return true;
        }
    }
}
