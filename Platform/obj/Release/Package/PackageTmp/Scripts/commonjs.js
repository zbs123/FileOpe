var config = {
    ajaxUrl: ""
}
/**
    ajax 操作方法
*/
var ajax = {
 /**
    Get请求  返回json
*/
    getJson: function (url, data, fun) {
        $.ajax({
            url: config.ajaxUrl + url,
            data: data,
            type: "GET",
            dataType:"JSON",
            success: function (obj) {
                fun(obj)
            }
        })
    },
/**
    Get请求  返回String
*/
    getStr: function (url, data, fun) {
        $.ajax({
            url: config.ajaxUrl + url,
            data: data,
            type: "GET",
            success: function (obj) {
                fun(obj)
            }
        })
    },
 /**
    Post请求 返回JSON  
*/
    postJson: function (url, data, fun) {
        $.ajax({
            url: config.ajaxUrl + url,
            data: data,
            type: "POST",
            dataType:"JSON",
            success: function (obj) {
                fun(obj)
            }
        })
    },
 /**
    Post请求 返回String 
*/
    postStr: function (url, data, fun) {
        //console.log(data);
        $.ajax({
            url: config.ajaxUrl + url,
            data: data,
            type: "POST",
            success: function (obj) {
                fun(obj)
            }
        })
    }
}
 /**
    Url操作
*/
var url = {

    GetUrlToParameter: function (parName) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;

    }

}
 /**
    关于字符串验证操作
*/
var strVer = {
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
       // console.log(str)
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
}
 /**
       cookie 操作
 */
var cookie = {

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

}
/**
localStorage本地存储操作类方法
HTML5 支持
IE 9+ 以上版本
Chrome 35+
FireFox 12+ 
*/
var localStorage = {
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
}
/**
页面跳转的方法 
*/
var urlhref = {
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
       跳转到指定页面
     @param {String} url 要跳转的url 地址 可以是同域也可以是不同域
    */
    Gourl: function (url) {
        window.location.href = url
    },

}

/**
对数字的一些操作
*/
var num = {
 /**
 * 
 * @desc 生成指定范围随机数
 * @param  {Number} min 
 * @param  {Number} max 
 * @return {Number} 
 */
    randomNum: function (min, max) {
        return Math.floor(min + Math.random() * (max - min));
    }
}
/**
 * 操作json串
 */
var json = {
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

       // console.log(value)

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
    update: function (json, node, key, value, newvalue) {
        var sub = json[node];
        for (var i = 0; i < sub.length; i++) {
            var cur = sub[i];
            if (cur[key] == value) {
                cur[key] = newvalue;
            }
        }
        return json;
    },
    /**
    验证当前json中是否存在相同的model对象
    @param json json 数据
    @param model 指定的json对象
    */
    jsonContain: function (json, model) {
        for (var i = 0; i < json.length; i++) {
            if (json[i].uid == model.uid) {
                return true;
            }
        }
        return false;
    }
}

 