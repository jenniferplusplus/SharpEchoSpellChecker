using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SharpEcho.Recruiting.SpellChecker.Contracts;

namespace SharpEcho.Recruiting.SpellChecker.Core
{
    /// <summary>
    /// This is a dictionary based spell checker that uses dictionary.com to determine if
    /// a word is spelled correctly
    /// 
    /// The URL to do this looks like this: http://dictionary.reference.com/browse/<word>
    /// where <word> is the word to be checked
    /// 
    /// Example: http://dictionary.reference.com/browse/SharpEcho would lookup the word SharpEcho
    /// 
    /// We look for something in the response that gives us a clear indication whether the
    /// word is spelled correctly or not
    /// </summary>
    public class DictionaryDotComSpellChecker : ISpellChecker
    {
        /// TODO: HttpClient is expensive and needs to be disposed properly
        /// My first thought for how to do that safely is to make DictionaryDotComSpellChecker implement a singleton
        /// pattern. The singleton would then manage the HttpClient lifecycle.
        /// But, I think that is beyond the scope of this exercise.
        
        private static Lazy<HttpClient> _sharedHttpClient  = new Lazy<HttpClient>();

        private HttpClient _httpClient;
        private HttpClient HttpClient
        {
            get
            {
                _httpClient = _httpClient ?? _sharedHttpClient.Value;
                return _httpClient;
            }
        }

        public DictionaryDotComSpellChecker()
        {
            
        }
        
        public DictionaryDotComSpellChecker(HttpClient client)
        {
            _httpClient = client;
        }
        
        public bool Check(string word)
        {
            // TODO: Potential for injection vulnerability here if word is not from a trusted source
            var uri = new Uri(String.Format("http://dictionary.com/browse/{0}", word));
            var request = HttpClient.GetAsync(uri);

            // TODO: This is a little bit naive. A more robust check would be better.
            return request.Result.StatusCode == HttpStatusCode.OK;
        }
    }
}
