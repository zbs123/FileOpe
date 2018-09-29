//layUI
//导航--依赖 element 模块，否则无法进行功能性操作

layui.use('element', function () {
    var element = layui.element;
});

window.onload = function () {
    //左侧导航点击
    
   var listIndex;
    $(".left>a").click(function () {
        listIndex = $(this).text();
        if (listIndex == "全部文件") {
            $(".right-search").removeClass("layui-hide");
            $(".left>a").eq(4).removeClass("layui-hide");
            $("#rightBox").attr("src", "pageone1");
        } else if (listIndex == "文档") {
            $(".right-search").removeClass("layui-hide");
            $(".left>a").eq(4).removeClass("layui-hide");
            $("#rightBox").attr("src", "pageOne1?type=2");

        } else if (listIndex == "图片") {
            $(".right-search").addClass("layui-hide");
            $(".left>a").eq(4).removeClass("layui-hide");
            $("#rightBox").attr("src", "picture");
        } else if (listIndex == "其他") {
            $(".right-search").removeClass("layui-hide");
            $(".left>a").eq(4).removeClass("layui-hide");
            $("#rightBox").attr("src", "pageOne1?type=0");
        } else if (listIndex == "分享") {
            localStorage.removeItem("peopleNames");
            //$("#shareBox").find(".layui-colla-item").remove();
            var poepleBox = layer.open({
                id: "person",
                type: 1,
                area: [$(".right").offset().left-10+"px", '100%'], //宽高
                offset: ['60px', '0px'],
                shade: 0,
                anim: 2,
                closeBtn: 1,
                title: false,
                content: $("#shareBox"),
                end: function () {
                    $("#shareBox ul li").removeClass("active");
                    $("#shareBox ul li").find("i").hide();
                    $("#shareBox").hide();
                    $("#rightBox").contents().find(".btnsWrap").show();
                    $("#rightBox").contents().find("#shareBtn,.shareBtn").hide();
                }
            });
            //面包屑导航修改
            $("#rightBox").contents().find(".btnsWrap").hide();
            $("#rightBox").contents().find("#shareBtn,.shareBtn").show();
        } else if (listIndex == "我的分享") {
            $(".right-search").addClass("layui-hide");
            $(".left>a").eq(4).addClass("layui-hide");
            $("#rightBox").attr("src", "myshare");
        } else if (listIndex == "我接收的文件") {
            $(".right-search").addClass("layui-hide");
            $(".left>a").eq(4).addClass("layui-hide");
            $("#rightBox").attr("src", "myshare?category=receive");
        } else if (listIndex == "回收站") {
            $(".right-search").addClass("layui-hide");
            $(".left>a").eq(4).addClass("layui-hide");
            $("#rightBox").attr("src", "recovery");
        }
    });
    //$(".layui-nav-child a").click(function () {
    //    $(".layui-nav-item>a").eq(1).removeClass("layui-hide");
    //    listIndex = $(this).text();
    //    if (listIndex == "文档") {
    //        $("#rightBox").attr("src", "pageOne?type=2");
    //    } else if (listIndex == "图片") {
    //        $("#rightBox").attr("src", "picture");
    //    } else if (listIndex == "其他") {
    //        $("#rightBox").attr("src", "pageOne?type=0");
    //    }
    //});
    //$(".layui-nav-item>a").click(function () {
    //    listIndex = $(this).text();
    //    if (listIndex == "回收站") {
    //        $(".layui-nav-item>a").eq(1).addClass("layui-hide");
    //        $("#rightBox").attr("src", "recovery");
    //    } else if (listIndex == "分享") {
    //        localStorage.removeItem("peopleNames");
    //        //$("#shareBox").find(".layui-colla-item").remove();
    //        var poepleBox = layer.open({
    //            id: "person",
    //            type: 1,
    //            area: ['250px', '100%'], //宽高
    //            offset: 'lt',
    //            shade: 0,
    //            anim: 2,
    //            closeBtn: 1,
    //            title: false,
    //            content: $("#shareBox"),
    //            end: function () {
    //                $("#shareBox ul li").removeClass("active");
    //                $("#shareBox ul li").find("i").hide();
    //                $("#shareBox").hide();
    //                $("#rightBox").contents().find(".btnsWrap").show();
    //                $("#rightBox").contents().find("#shareBtn,.shareBtn").hide();
    //            }
    //        });
    //        //面包屑导航修改
    //        $("#rightBox").contents().find(".btnsWrap").hide();
    //        $("#rightBox").contents().find("#shareBtn,.shareBtn").show();
    //    } else if (listIndex == "我的分享") {
    //        $(".layui-nav-item>a").eq(1).addClass("layui-hide");
    //        $("#rightBox").attr("src", "myshare");
    //    } else if (listIndex == "我接收的文件") {
    //        $(".layui-nav-item>a").eq(1).addClass("layui-hide");
    //        $("#rightBox").attr("src", "myshare?category=receive");
    //    }
    //});
    //人员选择点击
    var peopleArr = [];
    $(document).on('click', '#shareBox .layui-colla-content>ul>li', function(){
        if ($(this).attr("class") == "active") {
            $(this).removeClass("active");
            $(this).find("i").hide();
            var people = $(this).find("span").data("userid");
            peopleArr.forEach(function (element, i) {
                if (element == people) {
                    peopleArr.splice(i, 1);
                }
            })
            console.log(peopleArr)
            localStorage.setItem("peopleNames", JSON.stringify(peopleArr));
        } else {
            $(this).addClass("active");
            $(this).find("i").show();
            var people = $(this).find("span").data("userid");
            peopleArr.push(people);
            console.log(peopleArr)
            localStorage.setItem("peopleNames", JSON.stringify(peopleArr));
        }
    });
    $("#search").off().on("click", function () {
        var search = $(this).prev("input").val();
        if (search) {
            $(this).parent().nextAll().find(".layui-colla-content").addClass("layui-show")
            $(this).parent().nextAll().find("li").removeClass("layui-hide");
            $(this).parent().nextAll().find("li").each(function (i) {
                var str = $(this).find("span").text()
                if (str.indexOf(search) == -1) {
                    $(this).addClass("layui-hide");
                }
            })
        } else {
            $(this).parent().nextAll().find(".layui-colla-content").removeClass("layui-show")
            $(this).parent().nextAll().find("li").removeClass("layui-hide");
        }
        
    })
    //新加 该页面样式
        $(".show-btn").off().on("click", function () {
            $(".seach-right-box").animate({ right: "0px" }, 300, function () {
                $("#close-box").off().on("click", function () {
                    $(".seach-right-box").animate({ right: "-280px" }, 300)
                })
            })
        });

        $("#seacher-btn").off().on("click", function () {
            window.frames['rightBox'].search($("#r-search").val());
            //seacher(); 搜索
        });

        $("#clean-btn").off().on("click", function () {
            $(".seacher-body").find(".s-input").val("")
        })


   

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
        , showHeight: 10
    });
});


