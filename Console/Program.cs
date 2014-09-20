using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using WebCrawler.Core;
using Indexer.Token;


namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Crawler crawler = new Crawler("Ressources\\Seeds.txt");

            //Console.WriteLine("Starting...");
            //crawler.Start(500);
            
            //Console.WriteLine("Finished crawling...");
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadLine();
            string file = File.ReadAllText(@"C:\Users\Stefan\Git\WebCrawler\Console\bin\Debug\16-09-2014 12.22.38\0a11ad10-b52c-42e7-a806-2d2f7cf1aab7.html");
            //string file = "<html><head><title>titel</title></head>\n<body><script type='text/javascript'>var _vwo_code=(function(){var account_id=26962,settings_tolerance=2000,library_tolerance=1500,use_existing_jquery=true,f=false,d=document;return{use_existing_jquery:function(){return use_existing_jquery;},library_tolerance:function(){return library_tolerance;},finish:function(){if(!f){f=true;var a=d.getElementById('_vis_opt_path_hides');if(a)a.parentNode.removeChild(a);}},finished:function(){return f;},load:function(a){var b=d.createElement('script');b.src=a;b.type='text/javascript';b.innerText;b.onerror=function(){_vwo_code.finish();};d.getElementsByTagName('head')[0].appendChild(b);},init:function(){settings_timer=setTimeout('_vwo_code.finish()',settings_tolerance);this.load('//dev.visualwebsiteoptimizer.com/j.php?a='+account_id+'&u='+encodeURIComponent(d.URL)+'&r='+Math.random());var a=d.createElement('style'),b='body{opacity:0 !important;filter:alpha(opacity=0) !important;background:none !important;}',h=d.getElementsByTagName('head')[0];a.setAttribute('id','_vis_opt_path_hides');a.setAttribute('type','text/css');if(a.styleSheet)a.styleSheet.cssText=b;else a.appendChild(d.createTextNode(b));h.appendChild(a);return settings_timer;}};}());_vwo_settings_timer=_vwo_code.init();</script>\n<h1>h1</h1>\n<h3>h3</h3>\nhello\n<div>\ndiv\n</div>\n<div>\n<p><strong>p-i-div</strong></p><div>div-i-div</div></div></body></html>";

            Tokenizer tokenizer = new Tokenizer();


            File.WriteAllLines("strip.txt", tokenizer.Tokenize(file));
            //Tokenizer.stripHtml(file);

            Console.ReadLine();
        }
    }
}
