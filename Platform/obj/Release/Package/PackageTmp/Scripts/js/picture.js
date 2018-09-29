layui.use('form', function () {
    var form = layui.form;

    //全选
    form.on('checkbox(layAllChoose)', function (data) {
        // var thisParent = data.othis[0].parentElement;
        // var checkBoxArr = $(thisParent).siblings(".pContent").find(".layui-form-checkbox");
        // if(data.elem.checked){
        //     $(checkBoxArr).addClass("layui-form-checked");
        //     $(thisParent).siblings(".pContent").find("input[type=checkbox]").attr("checked",true);
        // }else{
        //     $(checkBoxArr).removeClass("layui-form-checked");
        //     $(thisParent).siblings(".pContent").find("input[type=checkbox]").attr("checked",false);
        // }

        var child = $(data.elem).parents(".pToolBar").siblings(".pContent").find('input[type="checkbox"]');
        child.each(function (index, item) {
            if (item.checked != data.elem.checked && data.elem.checked) {
                $("#nums").text(parseInt($("#nums").text()) + 1);
            }
            item.checked = data.elem.checked;
            if (data.elem.checked) {
                $(child).parents(".pBox").css("border-color", "#1E9FFF");
            } else {
                $("#nums").text(parseInt($("#nums").text()) - 1);
                $(child).parents(".pBox").css("border-color", "#fff");
            }
        });
        hideCancelChoose();
        form.render('checkbox');
    });
    //单选
    form.on('checkbox(picForm)', function (data) {
        var oThisParent = data.othis[0].offsetParent;//得到checkbox原始DOM对象的父元素
        if (data.elem.checked) {
            $("#nums").text(parseInt($("#nums").text()) + 1);
            $(oThisParent).css("border-color", "#1E9FFF");
        } else {
            $("#nums").text(parseInt($("#nums").text()) - 1);
            $(oThisParent).css("border-color", "#fff");
        }
        var sib = $(data.elem).parents('.pContent').find('input[type="checkbox"]:checked').length;
        var total = $(data.elem).parents('.pContent').find('input[type="checkbox"]').length;
        if (sib == total) {
            $(data.elem).parents('.pContent').siblings('.pToolBar').find('input[type="checkbox"]').prop("checked", true);
            form.render();
        } else {
            $(data.elem).parents('.pContent').siblings('.pToolBar').find('input[type="checkbox"]').prop("checked", false);
            form.render();
        }
        hideCancelChoose();
    });
    //取消选择点击
    $("#cancelChoose").click(function () {
        $('.pContent').find('input[type="checkbox"]').prop("checked", false);
        $('.pToolBar').find('input[type="checkbox"]').prop("checked", false);
        $(".pBox").css("border-color", "#fff");
        $("#nums").text("0");
        hideCancelChoose();
        form.render('checkbox');
    });
    //隐藏取消选择按钮
    function hideCancelChoose() {
        if (parseInt($("#nums").text()) > 0) {
            $(".btngroup").removeClass("layui-hide");
            $(".cancelChoose").removeClass("layui-hide");
        } else {
            $(".btngroup").addClass("layui-hide");
            $(".cancelChoose").addClass("layui-hide");
        }
    }
});

layui.use('flow', function () {
    var flow = layui.flow;


    //按屏加载图片
    flow.lazyimg({
        elem: '#picFlow img'
        // ,scrollElem: '#lay_photos' //一般不用设置，此处只是演示需要。
    });

});

window.onload = function () {
    //图片框鼠标移入
    $(".pBox").mouseenter(function () {
        $(this).find("i.iconfont").show();
        $(this).find(".layui-form-checkbox[lay-skin=primary]").addClass("layui-hover-checkbox");
    });
    //图片框鼠标移出
    $(".pBox").mouseleave(function () {
        $(this).find("i.iconfont").hide();
        $(this).find(".layui-form-checkbox[lay-skin=primary]").removeClass("layui-hover-checkbox");
    });
    //展开按钮点击
    $(".openBtn").click(function () {
        var btnTxt = $(this).text();
        if (btnTxt == "收起") {
            $(this).parents(".pToolBar").siblings().hide();
            $(this).text("展开");
        } else {
            $(this).parents(".pToolBar").siblings().show();
            $(this).text("收起");
        }
    });
    //图片放大预览
    var _html;
    //$.each($(".pBox>img"), function (i) {
    //    var element = $(".pBox>img").eq(i).prop("src");
    //    _html = '<img src="' + element + '" alt="">';
    //    $("#lay_photos>p").append(_html);
    //});
    layer.photos({
        photos: '#lay_photos'
        , closeBtn: 3
        , anim: 5
        , tab: function (pic, layero) {
            console.log(pic) //当前图片的一些信息
            var fileurl = pic.src.toLowerCase().split('fileos')[1];
            var htm = '<i class="iconfont icon-shanchu delete" data-fileurl="' + fileurl + '"></i><i class="iconfont icon-xiazai down" data-fileurl="' + fileurl + '"></i>'
            $("#layui-layer-photos .layui-layer-imgbar").html(htm);
        }
    });


    //下载
    $(document).on('click', '.down', function () {

        var dataArr = [];
        if ($(this).data('fileurl')) {
            dataArr.push($(this).data('fileurl'));
        } else {
            $(".pBox input[type='checkbox']:checkbox:checked").each(function () {
                dataArr.push($(this).prev("img").data("fileurl"))
            })
        }
        if (dataArr.length > 0) {
            var loadli = layer.load(1);
            jQuery.ajax({
                type: "Post",
                url: "Export",
                data: {
                    pathArr: JSON.stringify(dataArr)
                },
                success: function (data, status, xhr) {
                    //parent.location.href = "/default/Export"
                    console.log(JSON.parse(data))
                    var res = JSON.parse(data);
                    if (res.code == "200") {
                        layer.close(loadli);
                        layer.confirm('确定要下载图片吗？', {
                            btn: ['下载', '取消'] //按钮
                        }, function () {
                            location.href = res.data;
                        }, function () {

                        });
                    }
                }
            })
        } else {
            layer.msg("请选择文件")
        }


    })
    //删除
    $(document).on('click', '.delete', function () {


        var dataArr = [];
        if ($(this).data('fileurl')) {
            dataArr.push($(this).data('fileurl'));
        } else {
            $(".pBox input[type='checkbox']:checkbox:checked").each(function () {
                dataArr.push($(this).prev("img").data("fileurl"))
            })
        }
        if (dataArr.length == 0) {
            layer.msg("请选择文件")
            return;
        }
        layer.confirm('确认删除吗？', {
            btn: ['确认', '取消'] //按钮
        }, function () {
            jQuery.ajax({
                type: "Post",
                url: "Recycle",
                data: {
                    path: JSON.stringify(dataArr)
                },
                success: function (data, status, xhr) {
                    //parent.location.href = "/default/Export"

                    var res = JSON.parse(data);

                    if (res.code == "200") {
                        location.reload();
                    } else {
                        layer.msg("删除失败");
                    }
                }
            })
        }, function () {
        });
    })
    $("#shareBtn").click(function () {
        var peopleName = localStorage.getItem("peopleNames");
        if (!peopleName) {
            layer.msg("请选择要分享的人");
            return;
        }
        var peopleNameJson = JSON.parse(peopleName);
        if (peopleNameJson.length == 0) {
            layer.msg("请选择要分享的人");
            return;
        }
        var filepaths = [];
        $(".pBox input[type='checkbox']:checkbox:checked").each(function () {
            filepaths.push($(this).prev("img").data("fileurl"))
        })
        if (filepaths.length == 0) {
            layer.msg("请选择文件");
            return;
        }
        $.ajax({
            type: "Post",
            url: "AddShare",
            data: {
                usersid: peopleName,
                filepath: JSON.stringify(filepaths)
            },
            success: function (data) {
                console.log(data)
                var res = JSON.parse(data);
                if (res.code == "200") {
                    var alert = layer.alert('分享成功', function () {
                        $('#layui-layer1', parent.document).hide();
                        window.parent.location.reload(); //刷新父页面
                        layer.close(alert);
                    });
                } else {
                    layer.msg('分享失败', function () {
                        //关闭后的操作
                    });
                }
            }
        })

    });
    //新加
    $(".show-btn").off().on("click", function () {
        $(".seach-right-box").animate({ right: "0px" }, 300, function () {
            $("#close-box").off().on("click", function () {
                $(".seach-right-box").animate({ right: "-280px" }, 300)
            })
        })
    });

    $("#seacher-btn").off().on("click", function () {
        //window.frames['rightBox'].search($("#r-search").val());
        //seacher(); 搜索
        search($("#r-search").val());
    });

    $("#clean-btn").off().on("click", function () {
        $(".seacher-body").find(".s-input").val("")
    })

    //弹出用户选择框
    var s_userids = [];
    $(".addVotePeople").off().on("click", function () {
        var _obj = $(document);
        var _left = (_obj.width() - 650) / 2, _top = (_obj.height() - 620 + _obj.scrollTop()) / 2;
        $("#shareBox").show().css({ "left": _left + "px", "top": _top + "px" });
        own.bg.show();
        $(".close_cloose").off().on("click", function () {

            close();
        })
    })
    //点击部门展开部门人员
    $("#personlist>li>a").off().on("click", function () {
        var _obj = $(this);

        var _dd = _obj.siblings("div");
        if (_dd.css("display") == "none") {
            _obj.find("i").css({ "-webkit-transform": "rotate(90deg)", "left": "3px", "top": "-3px", "-moz-transform": "rotate(90deg)", "transform": "rotate(90deg)", "-ms-transform": "rotate(90deg)" });
            _dd.show();
        }
        else {
            _obj.find("i").css({ "-webkit-transform": "rotate(0deg)", "left": "0px", "top": "-0px", "-moz-transform": "rotate(0deg)", "transform": "rotate(0deg)", "-ms-transform": "rotate(0deg)" });
            _dd.hide();
        }
    });
    //左移
    $(document).on("click", "#personlist>li>div>span", function () {
        var _obj = $(this);
        var uid = $(_obj).data("uid");
        for (var i = 0; i < s_userids.length; i++) {
            if (uid == s_userids[i].uid) {
                $layer.warning("右侧列表中已经存在");
                return;
            }
        }
        s_userids.push({ uid: uid, uname: _obj.text() });
        $("#wrap ul").append("<li  data-uid='" + uid + "' data-type='" + $("#userType").val() + "'><a class='pl-10'>" + _obj.text() + "</a></li>");
    });
    //右移
    $(document).on("click", "#wrap>ul>li", function () {
        var _obj = $(this);
        var uid = _obj.data("uid");
        for (var i = 0; i < s_userids.length; i++) {
            if (uid == s_userids[i].uid) {
                s_userids.splice(i, 1);
                _obj.remove();
                return;
            }
        }
    });
    //确定按钮
    $("#lay-confirm").off().on("click", function () {
        var peopleName = [];
        if (s_userids.length == 0) {
            $layer.warning("请选择要分享的人");
            return;
        }
        for (var i = 0; i < s_userids.length; i++) {
            peopleName.push(s_userids[i].uid);
        }
        var filepaths = [];
        $(".pBox input[type='checkbox']:checkbox:checked").each(function () {
            filepaths.push($(this).prev("img").data("fileurl"))
        })
        console.log(filepaths)
        if (filepaths.length == 0) {
            layer.msg("请选择文件");
            return;
        }
        var data = {
            usersid: JSON.stringify(peopleName),
            filepath: JSON.stringify(filepaths)
        }
        $.ajax({
            type: "Post",
            url: "AddShare",
            data: {
                usersid: JSON.stringify(peopleName),
                filepath: JSON.stringify(filepaths)
            },
            success: function (data) {
                console.log(data)
                var res = JSON.parse(data);
                if (res.code == "200") {
                    var alert = layer.alert('分享成功', function () {
                        close();
                        layer.close(alert);
                    });
                } else {
                    layer.msg('失败', function () {
                        //关闭后的操作
                    });
                }
            }
        })
    })
    function close() {
        s_userids = [];
        $("#wrap ul").html("");
        $("#shareBox").hide();
        own.bg.hide();
    }
    $("#keyword").keyup(function (e) {
        var key = $(this).val();
        $("#personlist>li>div>span").hide();
        $("#personlist>li>div>span:contains('" + key + "')").show();
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
}