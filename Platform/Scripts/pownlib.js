$(function () {
    onresize();
})

$(window).resize(function () {
    onresize();//
});

function onresize() {
    if (document.addEventListener) {
        var w = $(document).width();
        $(".index-box .left").css({ "left": (w <= 1600 ? (((w - (1030 - 150)) / 2) - 180) : (((w - (1330 - 250)) / 2) - 280)) + "px" });
    }
}

/**
   配置js所依赖的参数
**/
var config = {
    url: "/Ajax/",
    timeout: 20000, //配置ajax 超时时间，默认为20秒
    loadimgpath: "/images/",
}

/**
 *  方法封装
 */
var own = {
    /**
    * 验证类方法
    */
    verify: {
        /**
          验证手机号是否正确
        @param str 传入需要验证的手机号
        */
        isPhone: function (str) {
            var reg = /^1[3|4|5|7|8]\d{9}$/;
            if (reg.test(str)) return true;
            else return false;
        },
        /**
          验证身份证格式是否正确
        @peram str 传入需要验证的字符串
        true ： 格式正确，false： 格式错误
        */
        isCardNo: function (str) {
            var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
            if (reg.test(str) === false) return false;
            else return true;
        },
        /**
          验证email格式是否正确
        @param str 传入需要验证的email格式
         true ： 格式正确，false： 格式错误
        */
        isEmail: function (str) {
            var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
            if (reg.test(str) === false) return false;
            else return true;
        },
        /**
         * 
         * @desc   判断是否为URL地址
         * @param  {String} str 
         * @return {Boolean}
         */
        isUrl: function (str) {
            return /[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/i.test(str);
        },
        /**
           验证表单内容是否为空
           @param str 传入需要验证的字符串
           true ： 字符串为空， flase ： 字符串不为空
        */
        isNull: function (str) {
            if (str.length == 0) return true;
            else return false;
        },
        /**
          验证传入的字符串是否是存数字
        @param str 传入需要验证的字符串
        ture ： 是数字，false 不是数字
        */
        isNumber: function (str) {
            var reg = /^[0-9]+.?[0-9]*$/; //判断字符串是否为数字 //判断正整数 /^[1-9]+[0-9]*]*$/ 
            if (reg.test(str)) return true;
            else return false;
        },
        /**
           判断输入的字符是否为:a-z,A-Z,0-9
         @param str 传入需要验证的字符串
         ture : 在范围内， false 不在范围内
        */
        isString: function (str) {
            var b = false;
            if (str.length != 0) {
                var reg = /^[a-zA-Z0-9_]+$/; 
                b = reg.test(str);
            }
            return b;
        },
        /**
           判断输入的字符是否为中文
         @param str 传入需要验证的字符串
         ture : 中文， false ：不是中文
        */
        IsChinese: function (str) {
            var b = false;
            if (str.length != 0) {
                var reg = /^[\u0391-\uFFE5]+$/; 
                b = reg.test(str);
            }
            return b;
        },

        /**
         * 
         * @desc 判断两个数组是否相等
         * @param {Array} arr1 
         * @param {Array} arr2 
         * @return {Boolean}
         */
        arrayEqual: function (arr1, arr2) {
            if (arr1 === arr2) return true;
            if (arr1.length != arr2.length) return false;
            for (var i = 0; i < arr1.length; ++i) {
                if (arr1[i] !== arr2[i]) return false;
            }
            return true;
        },
        /**
         * 
         * @desc   判断`obj`是否为空
         * @param  {Object} obj
         * @return {Boolean}
         */
        isEmptyObject: function (obj) {
            if (!obj || typeof obj !== 'object' || Array.isArray(obj))
                return false
            return !Object.keys(obj).length
        }
    },
     /**
       cookie 操作
    */
    cookie: {
        /**
         * 
         * @desc  设置Cookie
         * @param {String} name cookie name
         * @param {String} value   cookie valus
         * @param {Number} days   cookie 过期天数 
         */
        set: function (name, value, days) {
            var date = new Date();
            date.setDate(date.getDate() + days);
            document.cookie = name + '=' + value + ';expires=' + date;
        },
        /**
         * 
         * @desc 根据name读取cookie
         * @param  {String} name 
         * @return {String}
         */
        get: function (name) {
            var arr = document.cookie.replace(/\s/g, "").split(';');
            for (var i = 0; i < arr.length; i++) {
                var tempArr = arr[i].split('=');
                if (tempArr[0] == name) {
                    return decodeURIComponent(tempArr[1]);
                }
            }
            return '';
        },
        /**
         * 
         * @desc 根据name删除cookie
         * @param  {String} name 
         */
        remove: function (name) {
            own.cookie.set(name, "1", -1);
        },
        /**
         * 
         * 清除所有的cookie
         */
        removeAll: function () {
            var keys = document.cookie.match(/[^ =;]+(?=\=)/g);  
            if (keys) {
                for (var i = keys.length; i--;)
                    document.cookie = keys[i] + '=0;expires=' + new Date(0).toUTCString()
            }  
        }
    },
    /**
       本地存储操作类方法
       HTML5 支持
       IE 9+ 以上版本
       Chrome 35+
       FireFox 12+ 
    */
    local: {
         /**
         * 
         * @desc 设置本地存储
         * @param  {String} name
         * @param  {String} value
         */
        set: function (name, value) {
            localStorage.setItem(name, value);
        },
        /**
         * 
         * @desc 根据name读取local内容
         * @param  {String} name 
         * @return {String}
         */
        get: function (name) {
            return localStorage.getItem(name);
        },
           /**
         * 
         * @desc 根据name删除local
         * @param  {String} name 
         */
        remove: function (name) {
            localStorage.removeItem(name);
        },
        /**
          清空localStorage
        */
        clear: function () {
            localStorage.clear();
        }
    },
       /**
        格式互转
        */
    convert: {
     
        /**
       将Array转为字符串
        @param {Array} arr 要转换的Array
        @return {String} 
        **/
        ArrayToString: function (arr) {
            return arr.join(",");
        },
        /**
          将string转为Array
         @param {String} 要转换的字符串
         @param {String} char 分割的字符
         @returns {Array}
        */
        StringToArray: function (str, char) {
            if (str.length > 0) {
                return str.split(char);
            }
            return str;
        }
    },
    /**
       url相关操作
    */
    url: {
        /**
           后退
        */
        back: function () {
            window.history.back();
        },
        /**
           刷新当前页面
        */
        f5: function () {
            window.location.href = window.location.href;
        },
        /**
           跳转到置顶页面
         @param {String} url 要跳转的url 地址 可以是同域也可以是不同域
        */
        gourl: function (url) {
            window.location.href = url
        },
        /**
          根据URL参数的名称获取参数的值
         @param {String} paraName  url 参数名称
        */
        getUrlPara: function (paraName) {
            var sUrl = location.href;
            var sReg = "(?://?|&){1}" + paraName + "=([^&]*)"
            var re = new RegExp(sReg, "gi");
            re.exec(sUrl);
            return RegExp.$1;
        },
        /**
         新打开一个tab标签跳转到指定页面
        @param{String} url 要跳转的地址
        */
        goUrlblank: function (url) {
            var link = document.createElement('a');
            link.target = "_blank";
            link.href = url;
            document.body.appendChild(link);
            link.click();
            document.body.removechild(link);

        }
    },

     /**
           关于字符串的操作
        */
    string: {
     /**
           删除字符串最后一位
         @param {String} s 字符串
        */
        delLastChar: function (s) {
            s = s.substring(0, s.length - 1)
            return s;
        },
         /**
           一个字符串是否包含另一个字符串
         @param {String} str  原字符串
         @param {String} s  是否被包含的字符串
        @returns {Boolean} true 包含，false 不包含
        */
        index: function (str, s) {
            var b = false;
            if (str.indexOf(s) >= 0) b = true;
            return b;
        }
    },

    json: {
        /**
          给json添加节点
         @param json json 数据
        @param node 指定添加的节点名称
        @param obj 添加的节点
        @returns 返回新的json
        eg : var json = { p:[] }

        调用 own.json.push(json,"p",{"id":123,"name":"张三"})
        **/
        push: function (json, node, obj) {
            json[node].push(obj);
            return json;
        },
        /**
          删除json指定的节点
        @param json json 数据
        @param node 指定添加的节点名称
        @param key  key 名称 
        @param value key 的值
        @returns 返回新的json

          eg : var json = { p:[{"id":"123"},{"id":"456"}] }

        调用 own.json.delete(json,"p","id","123")  删除节点 id = 123 的节点
        */
        delete: function (json, node, key, value) {

            console.log(value)

            var sub = json[node];
            for (var i = 0; i < sub.length; i++) {
                var cur = sub[i];
                if (cur[key] == value) {
                    sub.splice(i, 1);
                }
            }
            return json;
        },
        /**
          修改json指定的节点
        @param json json 数据
        @param node 指定添加的节点名称
        @param key  key 名称 
        @param value key 的值
        @param newvalue 新的值
        @returns 返回新的json
          eg : var json = { p:[{"id":"123"},{"id":"456"}] }
        调用 own.json.delete(json,"p","id","123","12345")  把节点id=123 的值改为 12345
        */
        update: function (json, node, key, value , newvalue) {
            var sub = json[node];
            for (var i = 0; i < sub.length; i++) {
                var cur = sub[i];
                if (cur[key] == value) {
                    cur[key] = newvalue;
                }
            }
            return json;
        }
    },

    /**
     * 
     * @desc 随机生成颜色
     * @return {String} 
     */
    randomColor: function () {
        return '#' + ('00000' + (Math.random() * 0x1000000 << 0).toString(16)).slice(-6);
    },


    /**
     * 
     * @desc 生成指定范围随机数
     * @param  {Number} min 
     * @param  {Number} max 
     * @return {Number} 
     */
    randomNum: function (min, max) {
        return Math.floor(min + Math.random() * (max - min));
    },
    /**
       显示或者隐藏遮罩层
    **/
    bg: {
        show: function (opacity) {
            $("body").append("<div id=\"ly\"></div>");
            $("html").css({
                "overflow-x": "hidden",
                "overflow-y": "hidden"
            });
            var re = {};

            var _opacity = "";
            if (typeof opacity == "undefined") _opacity = "filter:Alpha(Opacity=40);opacity:0.4;";
            else _opacity = "filter:Alpha(Opacity=" + (opacity*100) + ");opacity:" + opacity + ";"
            /*console.log($(document).height())
            console.log(document.documentElement.clientHeight)
            console*/
            if (document.documentElement && document.documentElement.clientHeight) {
                var doc = document.documentElement;
                re.width = (doc.clientWidth > doc.scrollWidth) ? doc.clientWidth - 1 : doc.scrollWidth;
                re.height = ($(document).height() > doc.scrollHeight) ? $(document).height() : doc.scrollHeight;
            }
            else {
                var doc = document.body;
                re.width = (window.innerWidth > doc.scrollWidth) ? window.innerWidth : doc.scrollWidth;
                re.height = (window.innerHeight > doc.scrollHeight) ? window.innerHeight : doc.scrollHeight;
            }
            document.getElementById("ly").style.display = "block";
            document.getElementById("ly").style.cssText = "position:absolute;left:0px;top:0px;width:" + re.width + "px;height:" + re.height + "px;" + _opacity+"background-color:#000000;z-index:30";

        },
        hide: function () {
            $("#ly").remove();//清除背景透明
            $("html").css({
                "overflow-x": "hidden",
                "overflow-y": "auto"
            });
        }
    },
    /**loading 等待 **/
    loading: {
        show: function () {
           // own.bg.show(0.1);
            $("body").append('<img  id="loading"   src="/images/loading.gif"  style="position: fixed; left:50%; top:30%;z-index:10000"/>');
        },
        hide: function () {
            $("#loading").remove();
        }
    },
    /**
      ajax 操作方法
    */
    ajax: {
        post: function (urljson, data, call, type, timeout) {
            var url = (typeof (urljson.url) == 'undefined' ? config.url : urljson.url);           
            $.ajax({
                url: url + urljson.action,
                type: "POST",
                data: data,
                dataType: type,
                timeout: (typeof timeout === "undefined" ? config.timeout : timeout),
                beforeSend: function (h) {
                    console.log('发送前')
                },
                success: function (data, textStatus, jqXHR) {
                    if (typeof call === "function") {
                         call(data);
                    }
                },
                error: function (xhr, textStatus) {
                    console.log('错误')
                },
                complete: function () {
                    LoadLayUi();
                    console.log('结束')
                }
            })
        }
    }

   
    
}

