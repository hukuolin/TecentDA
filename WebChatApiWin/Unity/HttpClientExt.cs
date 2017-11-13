using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Domain.CommonData;
using Infrastructure.ExtService;
namespace WebChatApiWin
{
    public class HttpClientExt
    {
       
        public static string RunGet(string url) 
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response= client.GetAsync(url).Result;
            //提取cookie
            HttpResponseHeaders msgH = response.Headers;
            string msg = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            client.Dispose();
            return msg;
        }
        public static string RunPost(string url,string formData) 
        {
            string result = string.Empty;
            HttpClient client = new HttpClient();
            HttpContent content=new StringContent(formData,Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            result = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            client.Dispose();
            return result;
        }
        static void FillHttpWebRequestHead(HttpWebRequest request, string head)
        {
            if (string.IsNullOrEmpty(head))
            {
                return;
            }
            string[] arr = head.Replace("\r", "\n").Replace("\n\n", "\n").Split('\n');
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s)) break;
                if (s.StartsWith("GET", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (s.StartsWith("POST", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }


                if (s.StartsWith("Cookie", StringComparison.OrdinalIgnoreCase)) continue;
                int x = s.IndexOf(":");
                if (x > 0)
                {
                    try
                    {
                        var key = s.Substring(0, x);
                        var value = s.Substring(x);
                        request.Headers.Add(key, value);
                    }
                    catch (Exception ex)
                    { }
                }
            }
        }
        /// <summary>
        ///可选择提供请求头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static string RunPosterContainerHeader(string url,string head,CookieContainer cookie=null) 
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            if (cookie != null)
                request.CookieContainer = cookie;
            FillHttpWebRequestHead(request, head);
            WebResponse response = request.GetResponse();
            Stream st= response.GetResponseStream();
            Stream stm = new System.IO.Compression.GZipStream(response.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
            StreamReader sr = new StreamReader(stm, Encoding.UTF8);
            /* 出现该情况是请求内容为服务器响应请求启用了压缩算法，在接收的时候需要进行解压
             
             "�\b\0\0\0\0\0\0�}�wG����+���!�\r�-�6�s�q&�\t˰d�}����e7Hjђ�\t�9�-���\t��@���sK�O�/|u�z����j<3�9/g&����֭�խ�M����m#�X9��l=�G�Ʌ��طc��-1(See�3\t��>��e)3&%�<۵�D_��4V.꾊�0��Į\r�a=_��Z:�Ƥ�^(���idPUFյ�1Cϫ�I���VΩC���K��/,���vjj})�Z)�\b+�W=0�J���A]���֍�ڄV��մ�u���$�R1C��]��~삋\v�����I0b����Z�Z�����1?�~M/�F��ה�ؠ���2j�X+i��ɹD)#�Y#�H%#3��/\r���)���Z��k��y�MޗB��B�Rlh}��VP>PD})��=�~��Ɔ�l�VP��ֲ^l��_%5�m9��ZszF�AB��k�$�LN+�\f57+���f*eI��Ԍjv0��f����\t��r��D*�*mh��'�c[��jδi�Ѷ���_��bVϵ�<��M$p�m�FI[%�[�4K�wL�����}�w�٪�o-F�m�Q�}��+k�c�����[��Ŝ�(��X�݋�D�T>��O��ֲR�,m���!�\fAG�Z�N�ƴ|��!�S�ĺQ��ּƥ���jAѲ����B_����3\"wpOI/tr���\v�G`�Jc�Zn|�2�R[�CU2�LO{���7٩��tu%�3Jo_{_�ڊ��.Y�F������C���e�\\\t��|>!g`;'ʆ��+�'��Q����Ra4|��s9��D7����9����`N�U�J_�?�V\n9}T+��]���I\v\\��%��_�P�#��b�\\@k �\t������%=W)��Kh_�S�e��^.�y􇡍�A��\tľԉ�Drf�����X7����)����>�[�%#m:KF�C,E�g���K�����M�K�w���3O�?�.=Y�=�4�t}\t�Rat���d���0��ڍc�ߦ돧��O��Б����e�n � ��#^O5\at���l���J��|ol�<w�6����x}�<�4��z�am�\fsn�F׷�a9Є����#޴E���l&E��t>�X\n��\f�hh@e���x��vݜ����\t��h_��\t��-�\b��6���\r�F�D��l��o�;�TZ�\b�`�E��32��z�Ƙ���VځHM��;�׆RI�h�,ś��~n�{����d���Ld��Q��'�ªa����DF�BIs��\aq�9�N5CU�Л-��lW�h��M�0�;x���2r���*O����n#$��!���ٿ�Z^�+�B�������J* �tDg���mO^I��jY޼����\"�c|0��\aܻ�K�t\n���!�����Tg�Ⱦ����v�p����x��.�u�hr6�gy�P��������S�~��N�p���4H�.\r��(!/��`Js�\f��)CG˃�A+�Am�\v<��B�+n��w�F�w����\"��Ksgk�o�*�^U/O�c�l��;��9��z�e�\t�Q��D�z=�e��8�y�(��f(#\f 꾶vc��p��h+�@1����6Nƺ8�Quz�:u�<}I]<gQ�b�\r��Ϟ&+Q�����:�B�:���Ja�h��砄���^, �p�d��=s��9���V�]A�G[��u-��o2��\n�J��jFޙ�q�)��ܳ��\"�h��o�\t�9���*H�+H�?���4�_v_�P�}��5��Y��+i�1��\n�H��%&#�>̎I�\t[��������L�Ï���o�^yf.mm�TCdQ�\b\t�D�j{�����G��(�qd.W$�фIc�+�ɶ���6˙�;y��읕�i�������|1���?�\a�rf\r��Ս����l2#3���Ow��f��Q*\"��vȞT��F<�M�@�j|\bZQVR���h��\bc!kd(æ��JF3�(dE��6a(N�\"������zh��OT_��f�U��\fh�S4��oh#�;��xq�Ak׎��Ha4���<�X�ؿ�ٝ���^<�U���\0����@����uR���OJH��^i�ZȨ��2�҆\\N�����*:_����+� _�(8��MJdhy��Cy�?X���\nK7&\r[��\b�;���j��XG�3�V�ѳj�\n9����s�\rC����,�1\f�W�����A�/ôfVg䡏x(yJ�`���KԩM�Q[}uO|��m���r�3�m��A����1�i-�����;�mF�x���\0q��s\vB�h�k%���H]�]��tv�^t���Jh\t������\aJh�cb��g_��(l�g��O{�ܠ��r� ��$�p�|B/���;5��Ѿ��\v-WVA^�m��&Ԏ�o^���.�����6��J��\a&C�7�*o�\b$k��\n�J9�����J)�M�*F\v\f�r��\a��w_>��z~���^|ҡ����ۤ��x��H��9�4b��~��q�|AP�,���B~z@ҹ���nRz�ڌ:���ƭ�9��b�s�,FYN�V{�悞2�zb�{\a�����d\0#B�)|���j1��!N���D\r*霵U쿆�'�\b���O�~��w\r4�Te���=5���\0�Mm�\n�\\u�q�o���>�ԕm��:|�[g��׎�_̇��XI��\t#���Z�(%D\b��\r9�¢��>wp�nԟ�@d������\r<@\v$`I��Ar����MR��Kp�{v\n\"���H��5���,��-��3�\0R�18��rp�V��ZN+�F��b�p0I����8�ǐ9q�1MQ�B|-q�\b��JYG8���PP�P4CE�q�*���'鬵+诃��2�����;��.[hl���Y���oZ��I�g��4@F���;��#\b�Z��q��L��nUԺ+M�e_�=�\a\0g�B���r��P��ފT�BU�s�o\b%�ZM�W\a���RX��S�~=��<��Ί~fs͛K(��v׶���s��h:�j�u-�n.�nC�2�P֘q�U\\��������+��f��Ǫ[h h���'�\01��A�5P�v]G�x>�\"�(n\"�����쫨Ɓj@��|j�s&���3z��E�<��r\0�-c7B��dK��&�I\fA���k�@C��E�s�{�� y�pg��7\n�������8@����Di���z����ٰw�����x�$2\bb%P��\fk\0���v�w0d���JeJ�BD�\vq\f{��\n@�\\�T`���\"��D���Ap<�,Pշ�g�N�_�ؼ���f!X�ؑ�c������5��xWu1�>hءw�&�s���4(Yz�]` �������UB�_�� 0m��޳sl��A��p�����y��*��\f�ʪ3��y׶�:[�\n�\b�b�pS��'�^Q7�s�t����r��E4�%$�ا8��.M���=5���Ӽ�]m߉��\n�=\t��)�İ��$KL���t���}R=z�c��iQ�(�����cG}���Ȝ~����*9f��d(j\fiE��HG0SɃP��L�޵\a?{9�����Ke�\f��lM������k�\\�������C�-��\a�i�.3�G�V8i^{���\r0�~��pĎ��a���\v�ም����}�zl:�\vF\n`�H;\\}�4�����>]{�c8nj�1��W�\tg�76D��ڭ��}hgܺ����c��p�~�[\f��|c��E@@Lο��C?\b��xmKM/��n< Ea8��\v��N] Ua8�2���?�]��Ȋ���{�}��I2�+�\"+�DT���ўx[}uU��h����=}�v�\0��c[>^�\t�u�<�v���Cu\0Ua8���N�~ ��p���X�Y w@����>�k��=2P�c�����Ȉ�\b��͗LDR\b�O��Â������8\fRDU��C�����\nUa8�ȝ嫢��c����A�;��0����O��8�ci�ʅ���\t���ؖ�]`a]��N���4`T�/���biNt�vCW���w�؉H\n����埏\v0���p�8/�Z=uQ�\f��|��\\��a20*\fǶ|�pt!z\"p>�zgya^�\f�\nñ�p���Y2�'Ǯ���@.r�*\a���[��=���c��>2/.��\t�o٠>��u�*@ҲA�*.NU�#���ñ��z��h��8��Ǘ�WSd�9\f�\"��b���6f�����f��O|Dy�A^���O����\0�\t��8�y�&�~Q=4#�j�p,K��1�\t��8���EQ���0�����\vd�n�0\f�n��k�\n�A��p�\"��a�h��8�Awż,P&z��0�O�h=@XY]�v���@�������X���]�;=��0;ړg�/�[����o�K��\a��c�=��� X�D[�����}/@��p,U�<S{-zm8����v�7r?h�\0ǎ�Ȭ��N��\"�\"p�E۸�/����p'������_�W�.�(z��0�\r�@� #�\"p�|03�4'{�\n��|z���k��p8b8v�j��C/�-�v�����Wd��0��9&�\vs>4A�}���֪�w���\b0A�p,UEǅ`����0�O�{U=#X��N��\0����`����p��y-d�}@U�'�=7O\v��>�*\f�gwK�y���;� o�*@D�e+���/$DL[���f��@�o�ݴ��4o���y^ �����X�z��O�\f��8���W��V��X�8t�vX���AY�pl�1g�l?�W��������g ��Y>~#�����]��v�m�ۿ���� �ñ�����즠6b@�h��>%�/'ۓ ��O�z.$�d;���)��\aO�?\n��d{'��0,Gs�TGQݜ:����o�Fu�t��-8�!�����c��`1�w��\b\n��Rȗn׮!��&�\f�7��//��Q�zP����\"�����YQXs@/2Sm�\b-6�\"W��},B��\n2�~�:sS��� \0dZ�z���\bL��i��\a�b KĘ}�\0��#�p1��`(�͇�!B'ش9�[���\\�z��EPA\f�`\f��xU�9�\n����8�G!x���i����gG�l��%�)��a!6(l��^�3�?b�`\tA2�s'jo\t���<{D�@�>Z(����ge��Ȋ�1>h\t ���ag������0$�oG�X�-�#���g���\r*�d�g�P>�@lD_�!i�����8��\r��C�TDo\f��b/�����|��3�./~�2�`δ����ߥ|�\0�\f�Դ�pLx��@�!j��������j�5�\a�\"B���Ԝ/~_?�(D�<2\0����P \0�\fc��أ�u�Ba�\f�d�z�x�]\t�������Blp�`H��#����\b��.�!�]�@�cb\vÐ�l_��ऱ���@���A��>�v���G>������|\b���u�����\vǂ HV��}���m,o$˷�T��^��V�e.\v�7wBlb��Ϲk\rD�\r��n��V}q�vꝰ���������vT([�[�@23�6���\rC2��~gN�b�aH��ϣ`wBϟ���nb�C�̸_��οb�aH��_/�;/Ć�\f���5y��=�X<�����}�]�k�\fDu��\\g�*�Z9�\f~��I��d�\f���9�}%<0�/��X�3B<0�Ϟa��k!^76+0xo��\a�5Y^��/K�B�\a7\f�d���N��r�a��\t���|yk>�(��`HV�|�-\f�Ǿ/�t�!���G��$=g��\a?--\nw-xc\b$�i<*���t��W�r݄��=D#�� _N v/�����B�\v<2�m��ҜP��^\f���ͧ�p�H�,A;����\v,�M��A;*��{[�M(qb\r�d����L�A����~9];#TH�=C Y����;Bl�V��\f��_��\rZ$�dz~���Lx���@2�a����/���~�\b�5�7_��B��5��>Z=#����C2#>!<�[��X�e?�,�\"O\t\r��A`�\f��>J� aaH��o\"P�h\b$��O�\bQ�&��/����Z�� 0�Ե���3C@-e��I��li���;���2�aU/nR\n����0(�F?�Y~%�xaw\feF�E⯳�p��0��A�_��� d\0eF�\t\nF�(;-Ɠ�@c�Yi���꣩�ᑚc�Ͳ��谤�?�+?�\r�(��A\\C\a1)���a���.����\0�!��d�~�ŏ��F��`ԧxUE��n���._Z����E��P}y�\n1\n�jĠ^��bTD���%�9[��0qptbPf�/�G��Ԁ�[�ЬPN�EՀ}�\0�Gw�h��;\b:\t����;U���2�����Kb��\t(C.Qp���xh��-I0�P�`~~i��\r��v����\n��������b�\a�~�2�7���;�����:q�`�Gp̨=����t�!��Sg�?\n�$X�\t(Ci�^,�F5PeZ��X�ѱ(,��43����z�bj\b�0�`�^:��>�~��.\0\fʬ���̗��\abA\a�\04k�R�%>��7\0��J�å����e5�\v8�L%�^U��On���������1:��\0PVX�dޞ3_�/�8�\f!�8i���I�P�*�di�G1:p?\f�߉��)vp\r�d3bh�Rx?BTJ;\f0(ˁ��jIpP����n\v-:I�,��L�=���I:F�B�\\K\bF�o&P��~����M\f�S�̗�#u�\v+K�u۞2�D�`��aeW^ToE@\aS\teZ?6�|KL<�< ����`N��p P��q/R�\rbPf���~Sh=N�/��2�?��O�9�hVC����A_A��ϼ��8#F�\0\"\f�L���\b��a �>�Q�У�Ge:�i����������;\r�/��O�<P�����$��A��*4�%����|�(���|��x_GB'�\a�V1&\"��^���������MUo�ep6Pf~_?1O�5/����1�\n\b����h��w�A�G�BrH���l�`��7��nlȆ��t��ѽ�)z��<R��<��L�4{�&��Q7�����I�U݈A7�e�U+\r�q�}���$;��\";j- �v��y����\r����;����!�{�V�T}�<���I���ɯ��C���,�K��k�f��p�A�$5<7��l���ϳz=�|�T��O�×j������Yi�ɐF�v����¹-�ɂ�Kv��\"�D5$�\f�;�_������h�|%�I��^���\nd�*%��')8��\v�z]��t?�$_賝q3>\t�#��w%)�J2�+���Ќ�M؊&X���a�>��dk���`s�b�Ox=�\\�N�[\"I7\r7S�d$Md�\\����%.Fe<Ld\tx�\t�ri��es_V[:�,;6\f��Y7ǟM\0�wai�_kN-�����\bX��/!��>7��p�\v�j^͐V��B|y�[�t�}��t�~ǅ;���\ay&'[���+\n�io��� \\i$U]VU����.�\"�����9}n���W�ie�\t�B��~.͖�Cp����O�;��[V��܆�D`KF>e���]qX7�ew��V�]�rAB�wVΓ��\0�G����:PRI����Vހ�k1�mΛ-����'�˩q�\\m\fr�&K$��?�e�}�.���\f[����ʝ�$�<���b����� ]�f���;���#�piUc��������jfLu^����uN���$����L��&���kk\nOJ�GN�x�)���J=x�²�K��k=��@�i\\�#�IK�K��s�c:��=�c������`ȳ�v��O�8`�{uAY>#N'�~�v»�������3~4}W���f��|e�0���ک�љ*E9�+�#Z��ܻ�ۂ��$��P�'!A~Z��y���ϓ\a���Ѹ=��m�w\nP�@.d���W��O��C�YZa��#�W����ȬH>�8o��8�*r�%�K�?J�5xTim4�.�W�e;l���t������\0L3���縠=��y\\����Z��$'��Ϟ1�_�\"�8_I�b߄�ղ�\f�;����Vs�j�\vʚ�AcR��ϓ��O����\0�!���E��y�\\`��|�s�\\mWўï��{���\" w�<cA�l�V���F͟����ٹ��6����d�\f#�lg�\t��;?8��ca��y\0��\bS6��vd���3L�>U%�>�W38�\b{싑��gix��9����YjM���iMA�Kc�R\n�-\\%T����Q|_�,u`%�#̳�<ii������ޯ-��B���2�Z覐I\t���RNϸ�qs��\n�5x?5��\0��D� ~����9�'�v��Sn�FF�\"�$��]���r4N��7+��>2b�\"�,l���e�I+0L�z�� ��5�y-LN�[$��}Z����:���ί���{�r­*QҾU\a����℔�E����|�)�T\v~#�'s\n�4� Hܐ;?\"�|�O��k���\a�Wax7�����(�c��<��r��6��$<g!B��<�:vŌ�4�#\t�Y��-�ܐ\v\n%&�s�<�I�&�\b2�'5�Lv=�c�9��o����+�L�A�a��/߼^��Z5��ͥQ��Cd��.Y\0��\"$����y�#��3���ۏ\va)]�Sȸ�QC\r�LO]~8���l��a�\08���u�Ώ�d�|�j��E�gLX��Ə뺇�;'���#�J\0�ڌ]�2�_��2Ϭ%\\� �e��R��6a�K��-�5��^�LM+'�1E����ʺjW,TT��#�1MQ��ڶSYSE.�Ow�v�)Ro��%A���g�Ȣ����Vh��FWO\r�C����i��U咼7�y��K<�|���蘨�H������Ѩ��}b8\v�%q����,pP��jh���uv>�(z�}Z�X�4XE�ފ��OĨ���Ή-�s|Z��a���8옥���u�z��V(���\r+� tb��#AI��Es��'��U���٬���Lj����\r,_X\f���i�h<4l��1s��'[��l��W�drzIm�(#�ؙ#Z�]\r�$�\aSG�_=�\n���.gt̲\v\\(<u�u�\n����M\rO0�!��?��3�����d� �˜8fou�`l�{A�OF���`�ټy#��p��!ɖ��S��g�'68���4�X�\r���1���DJc�*0a:)R�DЌ:�������hH8�҇�U���Kyy���\rƒ=�\aZ}�$�:Y�i�b�;bd��B�)��R�߁�|�+\a���|)���N��LHx6�q��2/�I����{9�s�T9��cnH*7�\ng�p'��q�3�3�DF��;!T�Ȭ���\ns\v��a���ƭ�ɭһ��X^�\f��ţ\0���|�F��BLRV��:��s1�ٰ�qnVL��xe�%ϛ֓�=Hol�x��t�=�1C>9t�%o�Ρ�m8�ݥ���%�#�^Ǒ�ҝ��]s7�w\t���J벡p���C \b���\t�� ��S��w�ogY���Ļ́�J�2?�\\�?�\0#nɀG��Y|�dL�;����\ty����F��VM����[��T7ov<��oz�<;ϪS6��Ԏ\rebJ���[�\f��|�qG���X�\0.n0h�H�\t��dQ[��I�寶��%�\f,^��$�3a�i�Y��íHy16���[Z֬�@��`@j�>��>���jA;����?H������[��lM[h�!���`����#L�}sB��WS~@?������/��Y-T<����C/��&Q%��(w\a�K�il�2���~�o�Ճr_a.r.�R� �\nl����Z\t�(�ʟ��W| ��׻=\voX/�ڴb\"�����O�/���\r\0E�����'�/�z�Z��q,pK!'�cû�M�u#���˪ت�.%��HMԄ�)�b��x;_�]coa���\v�~�sG3r�!*ud�P�.��H�� ���BK��\\e_\a��5<Ǹ�e��O���<�W��hCR�0�[\t����>�{�����3O�߿��̡��\v�ǈ?����\t<�+��\\��df�m��Ke����p�>sK��+��_����YE)��-�S�e�GL�T���45G\f���_BN��:r�E���\r\"b%S�b���PS��xY/\"��3��z�������p��Ɖ\r๋���#E%�CE��g^�������r���3��͛7j�j��S#1�En���3�͕��P�z�4t$��a��l$\b��嵂h�v Rc�F�/0�,������V��K\r2@L\n�t �W��)4TE����t�[t�uX��X���ȆL�5Eɩ��\r��z�^��_˩<��F�����'���)`��5������GL��(�u����Y�E�h���Yv�b(_�m�d���m��N;8w�7;�V��ס�El~N\b�\v�y+P���GE�@ء�R@5;�G�9�[�|ѺyǟS;F�lL�عa���M[��\fn��\tI z\0�A����I��~ȀL`�\nӷ�6@2�S��7���[w���x\0��\v��?��}v�\b�\b����_6fL����\"YB�mm����$ܠm��E6 �H�q�3U!���h��'���YCC4K�n���H\a��8а�s����\\�l�]Uv�_������@�b�\aZ�4��FV�!�R%\r�H��K��V����ĭ�I��������3[��һ����~�2�� u�#իw�WN�~��i1��3��v�h\\9՗<!�CgRX�����G�d�=�R9�ՍT�=��QPjuDd5�T�����.�?���j\tS#�R�hG\a�YR'�:x�i��@�B7�r9H��\">c�ջ��t�#�-\"�e\aG����q��}���L��s��]�P%ާ]�FӨ���\n\"�Փ�?�4���J����FT�}G�\r\v��ܦ<���\rb�0Ѻ�كq�*�Y��'�o���ݦ�P���xK|��4��0c�=�>ǲ�\n7ء4=�yC|�|���8pm츈41��B2�f\r�'$���g�1� _n���L4���'6�VʊBD���5;��]J�5�9��� �&A�d��!���O�UK��t����>�b���4AP��7C.�ބ���1X�&�@RQ�<�8o�7o�\vÞ\f�X�\bY\"�V���\"~�@s3ǆ'�ZK8Qǆ����S�t�ѫ���Uk��]�t�J1��*3��S슲�����;��4�t\nW��+\n��70M�J\\^bߗ/\0�ެ�J��[jp��5M�&�.�u�,�-�2�n��N}�^$\b�<�.NT�\t�5G�=xP�?9)!��z�}���1��-�_�\0��9��S��뭯U��[���a��\"�J�����i�_���bpl�9U.����=Ŷ�����y���u��3N�g�F�{U4T\v�w_�#��Ym�IG\\�AXP�=�d4,�O�\v�댲��\"�n��H�l���K\"�b�UaiCITe��*�!\"�q���\nS�BP�d�\f�0���I��}qҬJvRU(%iY�d{��Oԣ����8�Ȳ\rH���H�E��d?��\b2\n��4MA[��ƨV�������A0���q�[�s���Al��:�����&.������{c\n�ش�T|�����[Z���_�VgNQ��{o1b��3O��$gth�{\0��8�Õ��$\\Z9bAT�0�;0�\r;��g�?��0�4�r\a\\A,@��u�qD\\���?�M�2��H�8�8�f~9�N~�I��q��D|QƩW�%�'���;�ی���bv{���W���7�#��c �@�S�R�h��g�s|!0$k�|pD����*�6��-��d1WN.��\t�9�S�2ҞR\n�J����G2*�����,i�^�L�qn&D�ڸu��]I2N<��C��wO��\"����8μ�AAFrBF��sr���ͤ/��o�/�v�w��=���E�TV�9uB8��\a�I]3'�j�6>G�<ڒu��I�Z.�7up�w���J��\v�d��\ně-�.Ce���2hh�f�RjL�U���mz�KR�s���\0�\\geM��ް�,g��p���R1ݳ��f�\f���G`���������B+�+º|�\"KQ�b�G���9��� �6��sH6n�p��ސO/�:��T�>�N�\\Όm4�b�]vT��h?R�c\0j��m�a\\W޳��J�+H>*���ltPE��cC��j�'�����p5��Wy&���mx��P��\fF� ���nn�K^8���a��Hl�A��;u�>�c�Ғ��J��X,�~@�2�\"J�fE\a\v�H��ܲ�g��<4�� �j�a|?���c��P,�f��&^\r\a�c�qiX�\f��,�`��!:�G��^S4�T�-����t�D���\v�t���e���O�!�3�ʊ��s�/0��ԶyC�\\�sD�^�oSU�\t��is������m�TH��t81>���v����/傒S�.x\b�f\0�u�J�@��Q|�X�@\to�V�a�]��$�&k3[V��-�jN+X���ai�!:m��^8�Wh=�׍��b��=b�-{����u��*�@�z�9�������L/(�f�`�^�\r4��m*����)�_�}����?�ς�YC�n\\p��\v��w��D�)\t��\b��w��D�̘o�����\\ٖ)\txU_~;c��~���_ƀ�oհ��_���G����ͤ��eW/#\a�}��D���_�Ԡ�[�~��Z~/Z<��8*1C�n:\f�;؞\rl�aن0�?c���Eh\t����{J)�鐒�o��T���\bȝ��m$5�y��M���^I%��\0f&<vk�Y��%�(�\fX�Ɇ��;/\fHl��nmض\r�C\"\v� l�]ۿBM q���M�7�y�y�\07o޾��MG�F�ƀ����~`jkAyw�E��C��NR#�H�0�(U���yG%mA|�]���Z�c��ְI�|(l���\a�l����a�\r��,V�$�1!���s��;i���\0�CG�b1����T8�M���+w�.�֝�\"R��Q�8�j}\r\t�U�ε4\v��Bn���aT8�\\E�F�Q.���!��Lbj�{��%��\v�<��/4�#Z���3�o�\"SAC�a����Ȗ�G�ںmdG�ؠ\\%�7�T�\0 ���\"��P%��`�_�!��6?L)�S\\�҇���t���g�H����?Κ��O���~�B��\r^Ħ������x�|fni�����BW���-��]۬h^��g\"t2�\a���p1�lH�?Y��e���u���'L|�ux��MH�o4$�h���;������]���=m����Z��\"�_�(����CzF������IG&�i�5�d�8��#>�n��|��\a�A=�J�r)R��Ο`�9Ad�\t�m�@��7BV�=�����³t?�\\����L�F� m�-3��������/Ta��Fk���\b�OH����X�$9Md��լn�;\r�P*��dA�[S�2�v�Yȱ-��q�!��\rsF��|#���T��w8i�xnW��bE%\n7E\v�$�'�_C �&��\vhE��Ʒ��x�a�αJ>m�\"�ȏ6{�em�����q0�HCX�g�j�U�]��1���,��` $���*Y>��,VJc��m�~�ƙDʈ�vO��\r�4��(I�A��\"�G�U���/G~�\t\af�� Jp�t�n&���9���ZNe�y3Y�/6}5b/�6}���q�Vp��_<[��z����[7\rG���u�\n\tr�1ష�@W��G��?톾��p�Y8L<7�?a\0��Ci, �w��4���?<H�@�cP����CD��Z�'ek�Dl*�_7�n��y�� JŰ�w�SS���m����8���q�\n�\b&]\v����~|b��� �T�}U\b�cQ\b[ST�5\n\r�\foc-��8�;�c$&���{lq����׼D�&!��#�s��k�Hֿ��#�����X��D:�`������?��#M���_���c�jEp�7��̗�4�[\r��zXv�Y�z<��S�S,<����|�\b~�~�mt)&,|)�%l9\fѕ|\tC�O�vQ/�!0&O�őS`o@<��p<�&�WpB�P8xF\vf'\f&��Ű�yH�!�K@��ϭD�l����\r�G�7l��G�]E[>�$�M��.~���=�F����wD݌X���{_��Vn�ei�9��Lp�Y%1���ӥ�9\n9���1�\0`����`���]��?����=m4�I\b�v�Aq�+��������<���1�5%~��=�{�8f)̈a�Vl��\0ՙ3���p���G�o��ѯS>0DOG����#��IE��ܤ�\r:��1%n+��qW8�+��r�ߗ/��~�p�wz�q7A[g�e�#������+F��%���K9t�WN5�-�C�Q\v�lз�?���&\twF@\nz��S���_�ɋ\\0M?��u��ǂf\vO�F�)\tQ�x\r¨�������m�o�Ν���p�����1x%�A��j\0��p�����|��e�=#�&��q\bH@mE%�\r\nV\\�D��\r\n>��\t_u+�O�3���cj��U��+�u����m�W�G��[��\nV\\[���xk�\vW>f�2��E+�q�z�[��6�\"��R�[��y��|U�N�V���P �}ЋQda�E�e|V�C����� UD������e�1�F��]#�F6Fje%&X+?m��G�{�^OJ'��Qs��Y���]\f�{��ih\0*�Z�M���\a�\v���!�i�M\b��A�R@�Y+�J\v̅?�����N,͞�/.F:}ͺ�Ao�2<�Dr����7�yV�4�\b�<�TX�=��p�|���ac[�g�kp�~�6t������Ń�z!;d��^<O7D[�1����t�������\rPB$��t�g*�R���ک���\fWWE�>�a����r%���������_O_Kp/��y�Ωp��q�8q��?'hc��V^[\\G��ƏZ\b���Zv�ZE#]�\"ɋC8�y���lu�q-_;^1o~�z�4���ʷ9�e*��\b�\rQT���//;�  ��P+\nKp.�q��E\b��\t�R*��� )y���\t]�im4n[�Z��=�'^je���_^��a<���zt���FfL��2�c����Чha�BSz*��u�yeo��.}�Cݹ�?��x��%a�%��7Y�����$����K��ǩeuvvB>�3��$ƚ(q��zV���C�S�fϙS/ͳ���8V��X�qF��c�텓-x^u\fdq=}H\"���G�2��T�e���[��00����&Erc�%��s9�lO��\b�J�l%\0�w]�\a��W:�z��Bg�>u�\\8T�{v�ֽ�C7j���a�)=k���5�F���F���%�7�����V2�6�`��e\a$��-\v��[����ۯŃٯ�*y5AF��O��n�`�Mf��~{�!���\0V��*\\�%���S}l����9}n�є��v�<S��=v�+��k9O*����G��^�s�/:d��w_E���\b�4�{H�s尴��a�{m����߈��~-��+��63��O���\n�4��Z��d��A؂�D�~p�\f\b�ں�\v:<���8�|����:ۨ�\"PZX��+/kO�����f���r:�<�qDМY����uK��A//��J������Pё2�є\r�e�`���S�6�L�_Yѧ�.� y��ff�1R.�6n]��3\v63�<����� !l��H+/UgN�/�'� aMBt���\"]���\vs��g��ޙ���㿛�ΛǏ��ܩ�z�>��@|jm\\& $����2�:��ԃ\a�7��������#���w_�G���B�Ԋ0�Y�]{��k�\\�M�#�m_V����YC�h��[LU�rV\f�Hvg����\n�cB�a��\\p�4�\f��������Z�}��˲8�P�3����4�W�J#�'�f��)�K�u=�M��54)�\a��R*(���N}6T����V\n�6|�Q�$��>��\f�u�րq�N\\��6�!r���NY\\=gU�#.W��?uy��+�s%͟i��Iά ��_9T?�w���gR�6�k�=<�ߎ-Jmۉ��N>�\r��\a�zq[NΨ_�9�K\\:9YtcX��D����$)*�]:N��'��(�\v'(>�2��������&o����zd��uw:#&�g5#�S+�x\"���H�w����M6�'��r��ƴ�d���%�6Z�����g3�ڋR��`��A+�j\"�b'�N��iϠ��v�9�\a��^���|~S�ʒ,Cw�؈���\t^��\tD�����\"�*M�����pe��mR�LNn\f����Q����~�/�x�Z8�QD�Դ��M�oF�\n�3]#n�\rU��*�*\ry��~������-@*���\r�\0�*��\t�Wy�\r�<+�r@�|���\n�\"yٓ���A�7�IE��M�kQ++唛�ޝ+g:�z)<Aɿq=��<\r�$��ƫ�gjo��a0�h[��b�F)��>Ҟ��\\���J��;0��!�BkV߽��ڀ�V_��>�L�yL���Ƚ��yX��\vS1��ا��0��{_��k(룣9H����5�+�^z�R�1@�,[PA�K�$�K}~0�!�3P��^�`��ӊ�D��ȳ\r������S������_.�0�Z�#.�����>7���-���½h��-�m\a�ԉ�m��I�D�\0ߚ3S�.>�󙇩�d���l�DiU>B�M�b9���y�Բ_��ġ�\"�^k�\na���5z���z!���X{����W�+,L�CU`1J����d��$�J�>Y����p�xE�O�,�f\r�t�`\a���-ڨ��o��>��Z*!yǍ�!��DPވ&��5�;F�^�⋘�~8+����'M�����6�,\t�y��:�&B��_2&lL`5��,AVl�\aC#���\"N��-�I�0��l�d��+��ŵ�L���Enτ�|\bM^�Ԑ���c���{j����F����+o�q���\rIA�+Y�4���b\b�H���U��̩���$im�������_��p;x�l�dw��K��/4�G�H�|`�\t�D���'�B�bx(�-���'����?�]�D\r�9[)`�es�AϜ/\a�jҠt�Gq�F�\nMM1�H4�6nU;�Qqm^\b���?q����mryl ���XjxK�/0u�.���h+�i��*�F�@�O�)%�Z����wki<_��m�F�-��*n�[���\rL$��v���Ee�vi�Ӓ������F�t���V�f�jU��A�0\rM���Ƒ��,ۉY[�t�Fa޻5ی6��X�4$%�.�dKs�:[���B?eYT5�DQ�~�֖\aN�+\f:兵*��\r��h%�dZ{ۯӲ��P�����͠�N͕T\t%\a\a���菘��ϣ5\r��Y��b�`З\0c]V7��bN�B\v�\\�:^�f�EMc�?˻�o\v�_-��Bl̚��k�f)5Xh�D�N5��w(�}���@�:}��`.�6֮td3TT{rƜ�6�n��Ϙg��{'�X8[;��<9�xO��s�m��Kp�ߺ������>%����1���ɹ�K�&d*�\b������/.#�>��#��\r⊉��|p���\tT)��vu�C�.T��c\b>sƜ��;�>(z;o��c>\\4�ϐ��=I���*������g-n�Ӝ�\b�ʆ�/�*�t���RlaL������NJ�?_a3+���{��.��N(�����?T��pi��&.^�����wd�=�2^=����%�Кo���VoA�^zQ={��ki�\\�⹥�����Tզ��o���S��'�s�\b�2�쐻3@\0t����j��.=Y�;A�N�����GG�?� ���k�_��O���ah2�#��2ݽ��roC#G�+�,�������nN�4��j�`a�}]jWV&D�um���]��N��3�M�>��������G��a\v�u�v��J�#ߊ�D$���C]����׆�#�.��8�ȋ��ߎU�\\G����dwҥF|;ƑV��25WS/�ݨ?a.Evt�w�w�jx`���_ԙ�If�{���ˈ6P�ү�{�ݨ��2�!��\v�����+�r.J�\b%X��5��H\\��VRM���&���PQ�KF���;����Pݽ��Vu�P��w�:���dS����DB�NM���3������=�*L�nB��@\v}����n�\\\n��{��x��&<���]�d�������=�ݓ%�x��!�Xr]�b\tԓd����C����]��;Z�pU�\\�L�{�I��<A�OYE�;�N���C�[wI���8�T��\vk��Ӌw�n����|�ue;Pq7���%D,�r���B�OU\t�q�!,Ê�B�Nwt�epK8�\r0{Y�U2�gq�2������#\v����x`��ήl�7IK\vתG�f��Ч�we�nr�� jX��s� \rfi�� l�Ʈ��݋w֋w�+�(�Mv�����Pe��fՎvv,�S?��O����ٍ:�-��4\"z�қ�J�}�IiT���JԎ.�\v��\aM��*}�9������l2��x\a*K�rOf�������K4�w\\��#�����YRs��'�s�Q�Ф�����G��\v̷���t�_�;�ٗl��'�N�2��7�-_���ګfz0��h�N��E��Oh%`Ou��g:`0����)r��y�J$�te�0�!�t��&;�iLz�k��wN�#�IJG?fBNQW�]N�����6\f��YU%'��% s��px;��l:ۓ�r��3�[��v:2Z@\bp�d\f+�����'A\"#�\tw��!��v�E��z��dO{g�����Ƨ��#�[PKsp�=�ݪ\ru\n{�̛7�*�gT�F��EDԝ촠�?�M��s�'[oЧ�_Eʉa�'���{;��=8/��ռ�ԆU��ڎH�Of*mK��0.��:��\0q֊S\0"
             
             */
            string text= sr.ReadToEnd();
            /*
            查询结果
             <error><ret>0</ret><message></message><skey>@crypt_45ae67ad_38b3f61167869aa2d95ec03017fc3b1b</skey><wxsid>qF/6PWxWsfeg/ocY</wxsid><wxuin>2266323382</wxuin><pass_ticket>F6FGYITm40nVvlvflrqhdclefVDWwmFquTZrXRah9CKwCZhHCqtqFmiQNYg2t93W</pass_ticket><isgrayscale>1</isgrayscale></error>
             */
            sr.Close();
            response.Close();
            string dir=ConfigItem.LoggerDefaultDir;
            if (string.IsNullOrEmpty(dir))
                dir = NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            LoggerWriter.CreateLogFile(text, dir, ELogType.SessionOrCookieLog);
            return text;
        }
        public static string RunPosterContainerHeaderHavaParam(string url, string head,string json, CookieContainer cookie = null)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            if (cookie != null)
                request.CookieContainer = cookie;
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            byte[] jsonStream = Encoding.UTF8.GetBytes(json);
            request.ContentLength = jsonStream.Length;
            using (Stream requestSt = request.GetRequestStream())
            { //进行请求时的流信息
                requestSt.Write(jsonStream, 0, jsonStream.Length);
                requestSt.Close();
            }
            FillHttpWebRequestHead(request, head);
            WebResponse response = request.GetResponse();
            Stream st = response.GetResponseStream();
            StreamReader sr = new StreamReader(st, Encoding.UTF8);
            string text = sr.ReadToEnd();
            sr.Close();
            response.Close();
            string dir = ConfigItem.LoggerDefaultDir;
            if (string.IsNullOrEmpty(dir))
                dir = NowAppDirHelper.GetNowAppDir(AppCategory.WinApp);
            LoggerWriter.CreateLogFile(text, dir, ELogType.SessionOrCookieLog);
            return text;
        }
    }
}
