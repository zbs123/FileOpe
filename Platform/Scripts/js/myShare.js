//表格数据
layui.use('table', function () {
    var table = layui.table;
    jQuery.ajax({
        type: "Post",
        url: "getmyShare",
        data: {
            category: $("#category").val()
        },
        success: function (data) {
            var res = JSON.parse(data);
            console.log(res)
            jQuery("#listcount").text(res.count);
            var newArr = res.data;
            var newContent = new Object();
            //获取当前系统日期
            var mydate = new Date();
            var date = "" + mydate.getFullYear() + "-";
            date += (mydate.getMonth() + 1) + "-";
            date += mydate.getDate();
            //表格
            if (newArr.length > 50) {
                table.render({
                    elem: '#test'
                    , data: newArr
                    , limit: 50
                    , page: {

                        layout: ['count', 'prev', 'page', 'next']
                        , prev: '上一页'
                        , next: '下一页'
                        , count: newArr.length
                        , limit: 50
                        , theme: '#1E9FFF'
                    }
                    , cols: [[
                        { type: 'checkbox' }
                        , { field: 'Name', title: '文件名', width: '50%', event: 'setSign', sort: true, templet: '#fileName' }
                        , { field: 'Size', title: '大小', sort: true }
                        , { field: 'Username', title: '分享人' }
                        , { field: 'Date', title: '修改日期', sort: true }

                    ]]
                    , initSort: { field: 'Date', type: 'desc' }
                    , done: function (res) {
                        if (res.count == 0) {
                            jQuery("#noData").removeClass("layui-hide");
                            jQuery(".layui-none").remove();
                            jQuery(".layui-table-page").remove();
                        } else {
                            jQuery("#noData").addClass("layui-hide");
                        }
                    }
                    , id: "test"
                });
            }
            else {

                table.render({
                    elem: '#test'
                    , data: newArr
                    , limit: 50

                    , cols: [[
                        { type: 'checkbox' }
                        , { field: 'Name', title: '文件名', width: '50%', event: 'setSign', sort: true, templet: '#fileName' }
                        , { field: 'Size', title: '大小', sort: true }
                        , { field: 'Username', title: '分享人', templet: '#userName' }
                        , { field: 'Date', title: '修改日期', sort: true }

                    ]]
                    , initSort: { field: 'Date', type: 'desc' }
                    , done: function (res) {
                        if (res.count == 0) {
                            jQuery("#noData").removeClass("layui-hide");
                            jQuery(".layui-none").remove();
                            jQuery(".layui-table-page").remove();
                        } else {
                            jQuery("#noData").addClass("layui-hide");
                        }
                    }
                    , id: "test"
                });
            }
            //监听表格复选框选择
            table.on('checkbox(tableTest)', function (obj) {
                //console.log(obj);

            });
            table.on('sort(tableTest)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
                console.log(obj.field); //当前排序的字段名
                console.log(obj.type); //当前排序类型：desc（降序）、asc（升序）、null（空对象，默认排序）
                console.log(this); //当前排序的 th 对象

            });
            //监听工具条
            table.on('tool(tableTest)', function (obj) {
                var data = obj.data;
                if (obj.event === 'yes') {
                    var childInput = obj.tr[0].childNodes[1].childNodes[0].childNodes[1].childNodes[3].value;

                    data.Name = childInput;
                    newArr.push(data)
                    newArr.forEach(function (element, i) {
                        if (element.Name == "new") {
                            newArr.splice(i, 1);
                        }
                    });
                    // 表格重载
                    table.reload('test', {
                        url: "NewFile",
                        where: {
                            path: jQuery("#filepath").val() + "/" + childInput
                        },
                        done: function (res) {
                            if (res.code == "0") {
                                location.reload();
                            } else {
                                $layer.msg('失败', function () {
                                    obj.del();
                                    newContent = "";
                                });
                            }
                        }
                    });
                } else if (obj.event === 'no') {
                    obj.del();
                    newContent = "";
                }
            });
            var shareDate = '';
            var shareDateFlag = true;
            //监听单元格事件
            table.on('tool(tableTest)', function (obj) {
                var data = obj.data;
                if (shareDateFlag) {
                    shareDate = data.Date;
                    shareDateFlag = false;
                }
                if (obj.event === 'setSign') {
                    console.log(data.Username)
                    // 表格重载
                    if (data.Type == "3") {
                        shareDate = data.Date;
                        table.reload('test', {
                            url: "getmyshare",
                            where: {
                                path: data.FileUrl,
                                shareid: data.Shareid,
                                type: "3",
                                username: data.Username,
                                date: shareDate
                            },
                            method: 'post',
                            limit: 50,
                            done: function (res) {
                                console.log(res)
                                jQuery("#listcount").text(res.count);
                                if (data.Shareid) {
                                    $("#shareid").val(data.Shareid);
                                }

                            }
                        });
                    }
                    // 表格重载
                    if (data.Type == "1") {
                        table.reload('test', {
                            url: "getmyshare",
                            where: {
                                path: data.FileUrl,
                                type: "1",
                                username: data.Username,
                                date: shareDate
                            },
                            method: 'post',
                            limit: 50,
                            done: function (res) {
                                console.log(res)
                                jQuery("#listcount").text(res.count);
                                if (data.Shareid) {
                                    $("#shareid").val(data.Shareid);
                                }
                            }
                        });
                    }


                }
            });
            var $ = layui.$, active = {
                sortName: function () {//按文件名排序
                    // 监听排序
                    table.reload('test', {
                        initSort: Name //记录初始排序，如果不设的话，将无法标记表头的排序状态。 layui 2.1.1 新增参数
                        , loading: true
                        , where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                            field: Name.field //排序字段
                            , order: Name.type //排序方式
                        }
                    });
                }
                , sortSize: function () { //按大小排序
                    table.reload('test', {
                        initSort: Size //记录初始排序，如果不设的话，将无法标记表头的排序状态。 layui 2.1.1 新增参数
                        , where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                            field: Size.field //排序字段
                            , order: Size.type //排序方式
                        }
                    });
                }
                , sortDate: function () { //按日期排序
                    table.reload('test', {
                        initSort: Date //记录初始排序，如果不设的话，将无法标记表头的排序状态。 layui 2.1.1 新增参数
                        , where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                            field: Date.field //排序字段
                            , order: Date.type //排序方式
                        }
                    });
                }
                , createFile: function () { //新建文件夹
                    //新增的数据
                    newContent.Name = 'new';
                    newContent.size = '-';
                    newContent.date = date;
                    newArr.push(newContent)
                    // 表格重载
                    table.reload('test', {
                        data: newArr,
                        limit: 100
                    });
                }

            };
            $('.menueList li').on('click', function () {
                var type = $(this).data('type');
                active[type] ? active[type].call(this) : '';
            });
            $('.table-top .layui-btn').on('click', function () {
                var type = $(this).data('type');
                active[type] ? active[type].call(this) : '';
            });
            var tipindex;
            $(document).on("mouseenter", ".un", function () {
                tipindex = layer.tips($(this).data("uname"), $(this), { tips: [2, '#78BA32'] });
            })
            $(document).on("mouseleave", ".un", function () {
                layer.close(tipindex);
            })

        }
    });
});


$(function () {
    if ($("#category").val()) {
        $("#down").removeClass("layui-hide");
    } else {
        //$("#down").addClass("layui-hide");
        $("#delete").removeClass("layui-hide");
    }
    $("#menue").mouseenter(function () {
        $(".menueBox").show();
    });
    $(".menueBox").mouseleave(function () {
        $(".menueBox").hide();
    });
    $(".menueList li").click(function () {
        $(this).css("color", "#1E9FFF");
        $(this).find("i").show();
        $(this).siblings().find("i").hide();
        $(this).siblings().css("color", "#4c4c4c");
    });

    //下载
    $("#down").off().on("click", function () {
        var t = "";//文件类型 1：文件夹  2：文件  3：多个文件
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        console.log(checkStatus)
        if (checkStatus.data.length == 1 && checkStatus.data[0].Type != "1" && false) {
            window.location.href = checkStatus.data[0].FileUrl;
        } else {
            var selectArr = [];
            if (checkStatus.data.length == 1) {
                if (checkStatus.data[0].Type == "3" || checkStatus.data[0].Type == "1") {
                    t = "1";
                } else {
                    t = "2";
                }
            } else {
                t = "1";
            }
            for (var i = 0; i < checkStatus.data.length; i++) {
                if (checkStatus.data[i].Type == "3") {

                    selectArr.push.apply(selectArr, eval("(" + checkStatus.data[i].FileUrl + ")"))
                }
                else {
                    selectArr.push(checkStatus.data[i].FileUrl);
                }
            }

            if (selectArr.length > 0) {
                var loadli = layer.load(1);
                jQuery.ajax({
                    type: "Post",
                    url: "Export",
                    data: {
                        pathArr: JSON.stringify(selectArr)
                    },
                    success: function (data, status, xhr) {
                        //parent.location.href = "/default/Export"
                        console.log(JSON.parse(data))
                        var res = JSON.parse(data);
                        if (res.code == "200") {
                            layer.close(loadli);
                            if (t == "2") {
                                $layer.confirm('确定要下载该文件?',
                                    //{
                                    //    btn: ['下载', '取消'] //按钮
                                    //    , end: function () {
                                    //        delFile(res.data);
                                    //    }
                                    //},
                                    function () {
                                        location.href = res.data;
                                    }, function () {

                                    });
                            } else {
                                $layer.confirm('文件将打包下载，确定要下载该文件夹吗？',
                                    //{
                                    //    btn: ['下载', '取消'] //按钮
                                    //    , end: function () {
                                    //        delFile(res.data);
                                    //    }
                                    //},
                                    function () {
                                        location.href = res.data;
                                    }, function () {

                                    });
                            }
                        } else {
                            layer.close(loadli);
                            $layer.msg("文件不存在");
                        }
                    }
                })
            } else {
                $layer.msg("请选择文件")
            }
        }

    })
    //删除
    $("#delete").off().on("click", function () {
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        console.log(checkStatus.data)
        var dataArr = [];
        for (var i = 0; i < checkStatus.data.length; i++) {
            if (checkStatus.data[i].Shareid) {
                dataArr.push(checkStatus.data[i].Shareid);
            }
        }
        if (dataArr.length == 0) {
            if ($("#shareid").val()) {
                dataArr.push($("#shareid").val());
            }

        }

        if (dataArr.length > 0) {
            $layer.confirm('确认取消分享吗？', function () {
                jQuery.ajax({
                    type: "Post",
                    url: "DelShare",
                    data: {
                        shareid: JSON.stringify(dataArr)
                    },
                    success: function (data, status, xhr) {
                        location.reload();
                    }
                })
            }, function () {

            })
        } else {
            $layer.msg("请选择文件")
        }

    })
    function delFile(p) {
        $.ajax({
            type: "Post",
            url: "delfile",
            data: {
                path: p
            },
            success: function (data) {
                //悄悄删除没有任何操作
            }
        })
    }


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
        var s_name = $("#r-search-name").val()
        var s_date_start = $("#start").val();
        var s_date_end = $("#end").val();
        var s_type = $("#r-search-type").val()
        console.log(s_name, s_date_start, s_date_end, s_type)
        search(s_name, s_date_start, s_date_end, s_type);
    });

    $("#clean-btn").off().on("click", function () {
        $(".seacher-body").find(".s-input").val("")
    })
});
