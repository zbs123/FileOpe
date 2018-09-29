$(function () {
   /* $(window).scroll(function () {
        if ($(document).scrollTop() > 300) {
            $("#returnTop").show();
        }
        else $("#returnTop").hide();
    })
    $("#returnTop").click(function () {
        var speed = 200;//滑动的速度
        $('body,html').animate({ scrollTop: 0 }, 500);
        return false;
    });*/
    if (browser() != "chrome") {
        $(".browsermsg").fadeIn(200)
    }

    layui.use('util', function () {
        var util = layui.util;

        //执行
        util.fixbar({
            bar1: false
            , click: function (type) {
                console.log(type);

            }
            , css: { bottom: 100 }
            , showHeight: 30
        });
    });

    own.loading.hide();
})

function LoadLayUi() {
    
    layui.use('form', function () {
        var form = layui.form;
        form.render('select');
        form.render('checkbox');
        //各种基于事件的操作，下面会有进一步介绍
    });
}
/**
 * iframe 打开页面
@param title 页面的标题
@param w 宽度 ； 可使用百分比
@param h 高度
@param url 地址
 */
function iframe(title,w,h ,url,top) {
    layer.open({
        type: 2,
        title: title,
        shadeClose: true,
        move: false,  //禁止移动
        resize: false, //禁止缩放
        shade: 0.4,
        area: [w, h],
        content: url//iframe的url
    }); 


    $(".layui-layer-iframe").css({"top": top+"px"}) /**为了兼容火狐浏览器**/

}
var $layer = {
    alert: function (title, con, icon) {
        layer.alert(con, {
            icon: icon, //0警告，1 成功，2失败，3疑问，4 锁定，5哭脸，6笑脸
            title: title
        });
        $(".layui-layer").css("top", "30%");
    },
    alertCall: function (str, call) { //弹出框，点击按钮回掉函数
        layer.confirm(str, {
            btn: ['确定'] //按钮
        }, function () {
            if (typeof call === "function") {
                call();
            }
            });
        $(".layui-layer").css("top", "30%");
    },
    confirm: function (msg, call, cancelcall) {//询问框
        layer.confirm(msg , {
            btn: ['确定', '取消'], //按钮
            closeBtn: 0 
        }, function (index) {
            if (typeof call === "function") {
                call();
            }
            layer.close(index);
        }, function () {
            if (typeof cancelcall === "function") {
                cancelcall();
            }
        });

        $(".layui-layer-dialog").css({ "top": "30%" });

    },
    close: function () {
        // layer.close($(".layui-layer").attr("times"));
        layer.closeAll();
    },
    warning: function (msg) {
        layer.msg(msg, function () { });
        $(".layui-layer").css({ "top": "40%" }); 
    },
    msg: function(str){
        layer.msg(str);
        $(".layui-layer").css({ "top": "40%" });
    },
    download: function () {
        layer.open({
            type: 1,
            title: '下载文件',
            closeBtn: 0, //不显示关闭按钮
            shadeClose: false, //开启遮罩关闭
            move: false,
            content: '<div class="layer-down-box"><span><img src="/images/loading.gif"/>数据文件正在生成中，请稍后...</span><div class="layer-btn hide"> <a class="layui-btn layui-btn-sm layui-btn-normal">下载文件</a><a class="layui-btn layui-btn-sm layui-btn-primary">关闭</a></div></div>'
        });
        $(".layui-layer").css("top", "30%");
        $(".layui-btn-primary").off().on("click", function () {
            layer.closeAll();
        })
    }
}
/**
 * 显示页数
 */
var currentPageAllAppoint = 1;
function rpage(totle, limit, call, ele) {
    console.log(ele);
    layui.use(['form', 'laypage'], function () {
        var form = layui.form, laypage = layui.laypage;
        laypage.render({
            elem: (typeof ele === "undefined" ? "page" : ele)
            , count: totle
            , theme: '#1E9FFF'
            , limit: limit
            , curr: currentPageAllAppoint
            , layout: ['count', 'prev', 'page', 'next']
            , jump: function (obj, first) {
                currentPageAllAppoint = obj.curr;
                if (!first) {
                    //work.list.get(obj.curr, _temp, _degree, _date, _key);
                    if (typeof call === "function") {
                        call(obj.curr);
                    }
                }
            }
        });
    })
}

/*比较两个日期*/
function comparisonDate(begin_date, end_date) {
    return ((new Date(begin_date.replace(/-/g, "\/"))) < (new Date(end_date.replace(/-/g, "\/"))));
}

function browser() {
    var browser = {};
    var userAgent = navigator.userAgent.toLowerCase();
    var s;
    (s = userAgent.match(/msie ([\d.]+)/)) ? browser.ie = s[1] :
        (s = userAgent.match(/firefox\/([\d.]+)/)) ? browser.firefox = s[1] :
            (s = userAgent.match(/chrome\/([\d.]+)/)) ? browser.chrome = s[1] :
                (s = userAgent.match(/opera.([\d.]+)/)) ? browser.opera = s[1] :
                    (s = userAgent.match(/version\/([\d.]+).*safari/)) ? browser.safari = s[1] : 0;
    var version;
    if (browser.ie) {
        version = 'msie';// + browser.ie;
    } else if (browser.firefox) {
        version = 'firefox';// + browser.firefox;
    } else if (browser.chrome) {
        version = 'chrome';//+ browser.chrome;
    } else if (browser.opera) {
        version = 'opera';// + browser.opera;
    } else if (browser.safari) {
        version = 'safari';// + browser.safari;
    } else {
        version = '未知浏览器';
    }
    return version;
}


function showMenu(obj) {
    var is = obj.attr("data-is");
    if (is == "0") {
        $(".layui-table .option ul").hide();
        obj.attr("data-is", "1");
        obj.find("i").css({ "-webkit-transform": "rotate(90deg)", "left": "3px", "top": "-3px", "-moz-transform": "rotate(90deg)", "transform": "rotate(90deg)", "-ms-transform": "rotate(90deg)" });
        obj.find("ul").fadeIn();
    }
    else {
        obj.attr("data-is", "0");
        obj.find("i").css({ "-webkit-transform": "rotate(0deg)", "left": "3px", "top": "-3px", "-moz-transform": "rotate(0deg)", "transform": "rotate(0deg)", "-ms-transform": "rotate(0deg)" });
        obj.find("ul").fadeOut()
    }
}