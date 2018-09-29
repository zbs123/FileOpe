//表格数据
var tableData =null;
$(window).resize(function () {
    if (tableData != null) {
        reloadTable(tableData)
    }
});
tableLayui();
function reloadTable(data) {
    var table = layui.table;
    var res = JSON.parse(data);

    var newArr = res.data;
    jQuery("#listcount").text("");
    jQuery("#listcount").text(res.count);
    var newContent = new Object();
    //获取当前系统日期
    var mydate = new Date();
    var date = "" + mydate.getFullYear() + "-";
    date += (mydate.getMonth() + 1) + "-";
    date += mydate.getDate();

    //选中的第几行
    var trIndex, oldName, imgSrc;

    if (newArr.length > 50) {
        //表格
        table.render({
            elem: '#test'
            , data: newArr
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
                , { field: 'Name', width: 620, title: '文件名', sort: true, templet: '#fileName' }
                , { field: 'Size', title: '大小', sort: true, align: 'center' }
                , { field: 'Date', title: '修改日期', sort: true, align: 'center' }

            ]]

            , done: function (res, curr, count) {
                currpage = curr;
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
        //表格
        table.render({
            elem: '#test'
            , data: newArr
            , limit: 50
            , cols: [[
                { type: 'checkbox' }
                , { field: 'Name', width: '50%', title: '文件名', sort: true, templet: '#fileName' }
                , { field: 'Size', title: '大小', sort: true }
                , { field: 'Date', title: '修改日期', sort: true, align: 'center' }

            ]]

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

        trIndex = obj.data.LAY_TABLE_INDEX;
        var checkStatus = table.checkStatus('test')
            , data = checkStatus.data;
        if (data.length > 0) {
            $(".btngroup").removeClass("layui-hide");
        } else {
            $(".btngroup").addClass("layui-hide");
        }
    });
    table.on('sort(tableTest)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        console.log(obj.field); //当前排序的字段名
        console.log(obj.type); //当前排序类型：desc（降序）、asc（升序）、null（空对象，默认排序）
        console.log(this); //当前排序的 th 对象
        table.reload('test', {
            initSort: obj.field //记录初始排序，如果不设的话，将无法标记表头的排序状态。 layui 2.1.1 新增参数
            , url: "getfile"
            , where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                field: obj.field //排序字段
                , order: obj.type, //排序方式
                path: jQuery("#filepath").val(),
                type: jQuery("#type").val(),
                search: jQuery("#search").val(),
                token: jQuery("#token").val()
            }, done: function (res) {
                console.log(res)
                //newArr = res.data;
            }
        });
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
                        //location.reload();
                        tableLayui();
                    } else {
                        $layer.msg('失败', function () {
                            obj.del();
                            newContent = {};
                        });
                    }
                }
            });
        } else if (obj.event === 'no') {
            obj.del();
            newContent = {};
        } else if (obj.event === 'sure') {
            var newName = $(this).siblings(".newFile").val();
            console.log(data)
            console.log(newName)

            $.ajax({
                type: "Post",
                url: "rename",
                data: {
                    orignFile: data.FileUrl,
                    newFile: newName
                },
                success: function (data) {
                    var res = JSON.parse(data);
                    console.log(res);
                    if (res.code == "200") {
                        //location.reload();
                        tableLayui()
                    } else {
                        $layer.msg(res.msg, function () {
                            var _oht = '<div class="layui-table-cell laytable-cell-1-name">'
                                + '<span><img src="' + imgSrc + '" alt="">' + oldName + '</span>' +
                                '<span class="toolBar fr"><i class="layui-icon">&#xe65f;</i></span>' +
                                '<ul class="toolList">' +
                                '<li class="down">下载</li>' +
                                '<li class="delete">删除</li>' +
                                '</ul>'
                                + '</div>';
                            $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _oht;
                        })

                    }
                }
            })
            //var _oht = '<div class="layui-table-cell laytable-cell-1-name">'
            //    + '<span><img src="images/xls.png" alt="">' + newName + '</span>' +
            //    '<span class="toolBar fr"><i class="layui-icon">&#xe65f;</i></span>' +
            //    '<ul class="toolList">' +
            //    '<li>分享</li>' +
            //    '<li>下载</li>' +
            //    '<li>移动到</li>' +
            //    '<li>删除</li>' +
            //    '</ul>'
            //    + '</div>';
            //$(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _oht;
            //$(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].dataset.content = newName;
        } else if (obj.event === 'cancel') {
            var _oht = '<div class="layui-table-cell laytable-cell-1-name">'
                + '<span><img src="' + imgSrc + '" alt="">' + oldName + '</span>' +
                '<span class="toolBar fr"><i class="layui-icon">&#xe65f;</i></span>' +
                '<ul class="toolList">' +
                '<li class="down">下载</li>' +
                '<li class="delete">删除</li>' +
                '</ul>'
                + '</div>';
            $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _oht;
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
            for (var i = 0; i < newArr.length; i++) {
                if (newArr[i].Name == "new") {
                    newArr.splice(i, 1);
                }
            }
            //新增的数据
            newContent.Name = 'new';
            newContent.size = '-';
            newContent.date = date;
            newArr.splice(50 * (currpage - 1), 0, newContent)
            // 表格重载
            table.reload('test', {
                data: newArr,
                limit: 100
            });
        },
        resetName: function () { //获取选中数据
            var checkStatus = table.checkStatus('test')
                , data = checkStatus.data;

            if (data.length != 1) {
                layer.msg("一次可重命名一个文件");
            } else {
                var trObj = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1];
                //ldName = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].dataset.content;
                oldName = data[0].Name.split(">")[1].split("<")[0]
                imgSrc = $(trObj).find("img").attr("src");
                var _ht = '<div class="layui-table-cell laytable-cell-1-name">'
                    + '<span class="newCreatebox">'
                    + '<img src="' + imgSrc + '" alt="">'
                    + '<input type="text" value="' + oldName + '" class="newFile">'
                    + '<button class="layui-btn layui-btn-primary" lay-event="sure">'
                    + '<i class="layui-icon" style=" color: #1E9FFF;">&#xe605;</i>'
                    + '</button>'
                    + '<button class="layui-btn layui-btn-primary" lay-event="cancel">'
                    + '<i class="layui-icon" style="color: #1E9FFF;">&#x1006;</i>'
                    + '</button></span></div>';
                $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _ht;
            }
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
}
var trIndex;
function tableLayui() {
    $("#uploadBox").hide();
    layui.use('table', function () {
        var table = layui.table;
        var laypage = layui.laypage;
        var laydate = layui.laydate;
        var currpage = 1;
        //日期范围
        laydate.render({
            elem: '#r-search-date'
            , range: true
            , done: function (value, date, endDate) {

                $("#start").val(new Date(date.year, date.month - 1, date.date).toLocaleDateString());
                $("#end").val(new Date(endDate.year, endDate.month - 1, endDate.date).toLocaleDateString());
            }
        });
        jQuery.ajax({
            type: "Post",
            url: "getfile",
            data: {
                path: jQuery("#filepath").val(),
                type: jQuery("#type").val(),
                search: jQuery("#search").val(),
                token: jQuery("#token").val()
            },
            success: function (data) {
                tableData = data;
                var res = JSON.parse(data);

                var newArr = res.data;
                jQuery("#listcount").text("");
                jQuery("#listcount").text(res.count);
                var newContent = new Object();
                //获取当前系统日期
                var mydate = new Date();
                var date = "" + mydate.getFullYear() + "-";
                date += (mydate.getMonth() + 1) + "-";
                date += mydate.getDate();

                //选中的第几行
                var  oldName, imgSrc;

                if (newArr.length > 50) {
                    //表格
                    table.render({
                        elem: '#test'
                        , data: newArr
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
                            , { field: 'Name', width: 620, title: '文件名', sort: true, templet: '#fileName' }
                            , { field: 'Size', title: '大小', sort: true, align: 'center' }
                            , { field: 'Date', title: '修改日期', sort: true, align: 'center' }

                        ]]

                        , done: function (res, curr, count) {
                            currpage = curr;
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
                    //表格
                    table.render({
                        elem: '#test'
                        , data: newArr
                        , limit: 50
                        , cols: [[
                            { type: 'checkbox' }
                            , { field: 'Name', width: '50%', title: '文件名', sort: true, templet: '#fileName' }
                            , { field: 'Size', title: '大小', sort: true }
                            , { field: 'Date', title: '修改日期', sort: true, align: 'center' }

                        ]]

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
                    if (obj.checked) {
                        trIndex = obj.data.LAY_TABLE_INDEX;
                    }
                    var checkStatus = table.checkStatus('test')
                        , data = checkStatus.data;
                    if (data.length > 0) {
                        $(".btngroup").removeClass("layui-hide");
                    } else {
                        $(".btngroup").addClass("layui-hide");
                    }
                });
                table.on('sort(tableTest)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
                    console.log(obj.field); //当前排序的字段名
                    console.log(obj.type); //当前排序类型：desc（降序）、asc（升序）、null（空对象，默认排序）
                    console.log(this); //当前排序的 th 对象
                    table.reload('test', {
                        initSort: obj.field //记录初始排序，如果不设的话，将无法标记表头的排序状态。 layui 2.1.1 新增参数
                        ,url:"getfile"
                        , where: { //请求参数（注意：这里面的参数可任意定义，并非下面固定的格式）
                            field: obj.field //排序字段
                            , order: obj.type, //排序方式
                            path: jQuery("#filepath").val(),
                            type: jQuery("#type").val(),
                            search: jQuery("#search").val(),
                            token: jQuery("#token").val()
                        }, done: function (res) {
                            console.log(res)
                            //newArr = res.data;
                        }
                    });
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
                                    tableLayui()
                                } else {
                                    $layer.msg('失败', function () {
                                        obj.del();
                                        newContent = {};
                                    });
                                }
                            }
                        });
                    } else if (obj.event === 'no') {
                        obj.del();
                        newContent = {};
                    } else if (obj.event === 'sure') {
                        var newName = $(this).siblings(".newFile").val();
                        console.log(data)
                        console.log(newName)

                        $.ajax({
                            type: "Post",
                            url: "rename",
                            data: {
                                orignFile: data.FileUrl,
                                newFile: newName
                            },
                            success: function (data) {
                                var res = JSON.parse(data);
                                console.log(res);
                                if (res.code == "200") {
                                    tableLayui()
                                } else {
                                    $layer.msg(res.msg, function () {
                                        var _oht = '<div class="layui-table-cell laytable-cell-1-name">'
                                            + '<span><img src="' + imgSrc + '" alt="">' + oldName + '</span>' +
                                            '<span class="toolBar fr"><i class="layui-icon">&#xe65f;</i></span>' +
                                            '<ul class="toolList">' +
                                            '<li class="down">下载</li>' +
                                            '<li class="delete">删除</li>' +
                                            '<li class="renameFile" lay-event="renameFile">重命名</li>' +
                                            '</ul>'
                                            + '</div>';
                                        $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _oht;
                                    })

                                }
                            }
                        })
                       
                    } else if (obj.event === 'cancel') {
                        tableLayui()
                       
                    } else if (obj.event === 'renameFile') {
                        trIndex = obj.tr[0].sectionRowIndex;
                        var trObj = $(this).parent().parent().parent();
                        //ldName = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].dataset.content;
                        oldName = data.Name.split(">")[1].split("<")[0]
                        imgSrc = $(trObj).find("img").attr("src");
                        var _ht = '<div class="layui-table-cell laytable-cell-1-name">'
                            + '<span class="newCreatebox">'
                            + '<img src="' + imgSrc + '" alt="">'
                            + '<input type="text" value="' + oldName + '" class="newFile">'
                            + '<button class="layui-btn layui-btn-primary" lay-event="sure">'
                            + '<i class="layui-icon" style=" color: #1E9FFF;">&#xe605;</i>'
                            + '</button>'
                            + '<button class="layui-btn layui-btn-primary" lay-event="cancel">'
                            + '<i class="layui-icon" style="color: #1E9FFF;">&#x1006;</i>'
                            + '</button></span></div>';
                        $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _ht;

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
                        for (var i = 0; i < newArr.length; i++) {
                            if (newArr[i].Name == "new") {
                                newArr.splice(i, 1);
                            }
                        }
                        console.log(newArr)
                        //新增的数据
                        newContent.Name = 'new';
                        newContent.size = '-';
                        newContent.date = date;
                        newArr.splice(50 * (currpage - 1), 0, newContent)
                        console.log(newArr)
                        // 表格重载
                        //table.reload('test', {
                        //    data: newArr,
                        //    limit: 100
                        //});
                        table.render({
                            elem: '#test'
                            , data: newArr
                            , limit: 50
                            , cols: [[
                                { type: 'checkbox' }
                                , { field: 'Name', width: '50%', title: '文件名', sort: true, templet: '#fileName' }
                                , { field: 'Size', title: '大小', sort: true }
                                , { field: 'Date', title: '修改日期', sort: true, align: 'center' }

                            ]]
                        });
                    },
                    resetName: function (obj) { //获取选中数据
                        var checkStatus = table.checkStatus('test')
                            , data = checkStatus.data;
                        layui.form.render();
                        if (data.length != 1) {
                            layer.msg("一次可重命名一个文件");
                        } else {
                            var trObj = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1];
                            //ldName = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].dataset.content;
                            oldName = data[0].Name.split(">")[1].split("<")[0]
                            imgSrc = $(trObj).find("img").attr("src");
                            var _ht = '<div class="layui-table-cell laytable-cell-1-name">'
                                + '<span class="newCreatebox">'
                                + '<img src="' + imgSrc + '" alt="">'
                                + '<input type="text" value="' + oldName + '" class="newFile">'
                                + '<button class="layui-btn layui-btn-primary" lay-event="sure">'
                                + '<i class="layui-icon" style=" color: #1E9FFF;">&#xe605;</i>'
                                + '</button>'
                                + '<button class="layui-btn layui-btn-primary" lay-event="cancel">'
                                + '<i class="layui-icon" style="color: #1E9FFF;">&#x1006;</i>'
                                + '</button></span></div>';
                            $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _ht;
                        }
                    }

                };
                //$(document).on("click", ".renameFile", function () {
                //    console.log("fksdjflksklsdkl")
                //    var checkStatus = table.checkStatus('test')
                //        , data = checkStatus.data;
                //    trIndex = 0;
                //    //var trObj = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1];
                //    var trObj = $(this).parent().parent().parent();
                //    //ldName = $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].dataset.content;
                //    oldName = data[0].Name.split(">")[1].split("<")[0]
                //    imgSrc = $(trObj).find("img").attr("src");
                //    var _ht = '<div class="layui-table-cell laytable-cell-1-name">'
                //        + '<span class="newCreatebox">'
                //        + '<img src="' + imgSrc + '" alt="">'
                //        + '<input type="text" value="' + oldName + '" class="newFile">'
                //        + '<button class="layui-btn layui-btn-primary" lay-event="sure">'
                //        + '<i class="layui-icon" style=" color: #1E9FFF;">&#xe605;</i>'
                //        + '</button>'
                //        + '<button class="layui-btn layui-btn-primary" lay-event="cancel">'
                //        + '<i class="layui-icon" style="color: #1E9FFF;">&#x1006;</i>'
                //        + '</button></span></div>';
                //    $(".layui-table-body .layui-table tr").eq(trIndex)[0].children[1].innerHTML = _ht;
                //})
                $('.menueList li').on('click', function () {
                    var type = $(this).data('type');
                    active[type] ? active[type].call(this) : '';
                });
                $('.table-top .layui-btn').on('click', function () {
                    var type = $(this).data('type');
                    active[type] ? active[type].call(this) : '';
                });
            }
        });
    });}

$(function () {
    $("#test1").off().on("click", function () {
        own.loading.show();
        $.ajax({
            type: "POST",
            url: "/home/GetAsposeOfficeFiles",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                own.loading.hide();
                console.log(result);
                //var officeInternetView = result[0];
                var viewUrl = result[1];
                var pageView = result[2];
                var tempPage = result[3];
                layer.open({
                    type: 2,
                    title: 'layer mobile页',
                    offset: ['0px', '0px'],
                    shadeClose: true,
                    shade: 0.8,
                    area: ['100%', '100%'],
                    content: viewUrl //iframe的url
                }); 
                //var paging = $("#paging");
                //if (pageView === "True") {
                //    paging[0].style.display = "block";
                //    $("#page-nav")[0].value = tempPage;
                //} else {
                //    paging[0].style.display = "none";
                //}
                //$("#CurrentDocumentPage").attr("src", viewUrl);
            }
        })
    })
    if ($("#person", window.parent.document).length > 0) {
        $(".btnsWrap").hide();
        $("#shareBtn").show();
    }
    //隐藏新建文件夹按钮
    if ($("#type").val()) {
        $(".cf").addClass("layui-hide");
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
    //表格行划过出现工具图标
    $(document).on("mouseenter", ".layui-table tr", function () {
        $(this).find(".toolBar").show();
    })
    $(".layui-table tr").mouseenter(function () {
        $(this).find(".toolBar").show();
    });
    $(document).on("mouseleave", ".layui-table tr", function () {
        $(this).find(".toolBar").hide();
        $(this).find(".toolList").hide();
    })
    $(".layui-table tr").mouseleave(function () {
        $(this).find(".toolBar").hide();
        $(this).find(".toolList").hide();
    });
    //工具图标点击
    $(document).on("click", ".toolBar", function () {
        $(this).siblings(".toolList").show();
    })
    $(".toolBar").click(function () {
        $(this).siblings(".toolList").show();
    });
    $(document).on("click", ".toolList li", function () {
        $(this).addClass("active").siblings().removeClass("active");
    })
    $(".toolList li").click(function () {
        $(this).addClass("active").siblings().removeClass("active");
    });
    //选中人员并分享
    var a = "成功";
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
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        if (checkStatus.data.length == 0) {
            layer.msg("请选择文件");
            return;
        }
        var filepaths = [];
        for (var i = 0; i < checkStatus.data.length; i++) {
            if (checkStatus.data[i].FileUrl) {
                filepaths.push(checkStatus.data[i].FileUrl);

            }
        }
        console.log(filepaths)
        if (filepaths.length == 0) {
            layer.msg("请选择文件");
            return;
        }
        var data = {
            usersid: peopleName,
            filepath: JSON.stringify(filepaths)
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
                    layer.msg('失败', function () {
                        //关闭后的操作
                    });
                }
            }
        })

    });
    //搜索
    $(".searchBox i").off().on("click", function () {
        if ($("#search").val()) {
            $(".cf").addClass("layui-hide");
        } else {
            $(".cf").removeClass("layui-hide");
        }
        var searchArr = [];
        $.ajax({
            type: "Post",
            url: "GetFile",
            data: {
                search: $("#search").val()
            },
            success: function (data) {
                var res = JSON.parse(data);
                var table = layui.table;
                if (res.data.length > 50) {
                    //表格
                    table.render({
                        elem: '#test'
                        , data: res.data
                        , page: {

                            layout: ['count', 'prev', 'page', 'next']
                            , prev: '上一页'
                            , next: '下一页'
                            , count: res.data.length
                            , limit: 50
                            , theme: '#1E9FFF'
                        }


                        , cols: [[
                            { type: 'checkbox' }
                            , { field: 'Name', width: 620, title: '文件名', sort: true, templet: '#fileName' }
                            , { field: 'Size', title: '大小', sort: true }
                            , { field: 'Date', title: '修改日期', sort: true }

                        ]]

                        , done: function (res) {

                            $("#listcount").text("");
                            $("#listcount").text(res.count);
                            if (res.data && res.data.length == 0) {
                                $("#noData").removeClass("layui-hide");
                            } else {
                                $("#noData").addClass("layui-hide");
                            }
                        }
                        , id: "test"
                    });
                }
                else {
                    //表格
                    table.render({
                        elem: '#test'
                        , data: res.data
                        , limit: 50
                        , cols: [[
                            { type: 'checkbox' }
                            , { field: 'Name', width: 620, title: '文件名', sort: true, templet: '#fileName' }
                            , { field: 'Size', title: '大小', sort: true }
                            , { field: 'Date', title: '修改日期', sort: true }

                        ]]

                        , done: function (res) {
                            $("#listcount").text("");
                            $("#listcount").text(res.count);
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
            }
        })

    })

    //下载
    $(document).on("click", ".down", function () {
        var t = "";//文件类型1：文件夹  2：文件  3：多个文件
        var obj = $(this);
        var fu = obj.parent().prevAll("span").eq(1).data("fileurl");
        var fuType = obj.parent().prevAll("span").eq(1).data("type");
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        console.log(checkStatus)
        if (checkStatus.data.length == 1 && checkStatus.data[0].Type != "1" && false) {
            window.location.href = checkStatus.data[0].FileUrl;
        } else {
            var selectArr = [];
            if (fu) {
                selectArr.push(fu);
                if (fuType == "1") {
                    t = "1";
                } else {
                    t = "2";
                }
            } else {
                for (var i = 0; i < checkStatus.data.length; i++) {
                    selectArr.push(checkStatus.data[i].FileUrl);
                }
                if (selectArr.length == 1) {
                    if (checkStatus.data[0].Type == "1") {
                        t = "1";
                    } else {
                        t = "2";
                    }
                } else {
                    t = "3";
                }
            }

            if (selectArr.length > 0) {
                var loadli = layer.load(1);
                jQuery.ajax({
                    type: "Post",
                    url: "Export",
                    data: {
                        pathArr: JSON.stringify(selectArr)
                        , type: t
                    },
                    success: function (data, status, xhr) {
                        //parent.location.href = "/default/Export"
                        var res = JSON.parse(data);
                        console.log(res)
                        if (res.code == "200") {
                            $layer.close(loadli);
                            if (t == "2") {
                                $layer.confirm('确定要下载该文件?',
                                    //{
                                    //    btn: ['下载', '取消'] //按钮
                                    //    , end: function () {
                                    //        delFile(res.data);
                                    //    }
                                    //},
                                    function () {
                                        $(".export-save").attr("href", res.data);
                                        $(".export-save").attr("download", "");
                                        $("#sp").trigger("click");
                                    }, function () {

                                    });
                            } else {
                                $layer.confirm('文件夹将打包下载，确定要下载该文件夹吗？',
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

                        }
                    }
                })
            } else {
                $layer.msg("请选择文件")
            }
        }

    })
    //删除
    $(document).on("click", ".delete", function () {
        var obj = $(this);
        var fu = obj.parent().prevAll("span").eq(1).data("fileurl");
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        var dataArr = [];
        if (fu) {
            dataArr.push(fu);
        } else {
            for (var i = 0; i < checkStatus.data.length; i++) {
                dataArr.push(checkStatus.data[i].FileUrl)
            }
        }

        if (dataArr.length == 0) {
            $layer.msg("请选择文件")
            return;
        }
        $layer.confirm('确认删除吗？', function () {
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
                        $layer.close();
                        $(".btngroup").addClass("layui-hide");
                        tableLayui();
                    } else {
                        $layer.msg("删除失败");
                    }
                }
            })
        }, function () {
        });
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
        console.log(s_name, s_date_start,s_date_end, s_type)
        search(s_name, s_date_start, s_date_end, s_type);
    });

    $("#clean-btn").off().on("click", function () {
        $(".seacher-body").find(".s-input").val("")
    })

    //$(".share-btn").off().on("click", function () {
    //    localStorage.removeItem("peopleNames");
    //    var poepleBox = layer.open({
    //        id: "person",
    //        type: 1,
    //        area: [$(".right").offset().left - 10 - $(".left").offset().left + 50 + "px", '100%'], //宽高
    //        offset: ['60px', $(".left").offset().left - 50 + "px"],
    //        shade: 0,
    //        anim: 2,
    //        closeBtn: 1,
    //        title: false,
    //        content: $("#shareBox"),
    //        end: function () {
    //            $("#shareBox ul li").removeClass("active");
    //            $("#shareBox ul li").find("i").hide();
    //            $("#shareBox").hide();
    //            $("#rightBox").contents().find(".btnsWrap").show();
    //            $("#rightBox").contents().find("#shareBtn,.shareBtn").hide();
    //        }
    //    });
    //    //面包屑导航修改
    //    $("#rightBox").contents().find(".btnsWrap").hide();
    //    $("#rightBox").contents().find("#shareBtn,.shareBtn").show();

    //})
    //人员选择点击
    var peopleArr = [];
    $(document).on('click', '#shareBox .layui-colla-content>ul>li', function () {
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
    //分类
    $("#typelist li").off().on("click", function () {
        var dd = $(this).data("guid");
        var token = getQueryString("token");//获取参数
        var tttttt = $("#token").val();
        if (dd == '0') {
            $(".export-save").attr("href", "/home/picture?token=" + tttttt);
            $("#sp").trigger("click");
        }
        if (dd == '1') {
            $(".export-save").attr("href", "/home/pageone?type=2&token=" + tttttt);
            $("#sp").trigger("click");
        }
        if (dd == '2') {
            $(".export-save").attr("href", "/home/pageone?type=0&token=" + tttttt);
            $("#sp").trigger("click");
        }
    })
    function setType() {
        var type = getQueryString("type");//获取参数
        $("#typelist li").removeClass("active");
        if (type == '2') {
            $("#typelist li[data-guid='1']").addClass("active");
        }
        if (type == '0') {
            $("#typelist li[data-guid='2']").addClass("active");
        }
    }
    setType();
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    }
    //弹出用户选择框
    var s_userids = [];
    $(".addVotePeople").off().on("click", function () {
        var _obj = $(document);
        var _left = (_obj.width() - 650) / 2, _top = (_obj.height() - 620 + _obj.scrollTop()) / 2;
        $("#shareBox").show().css({ "left": _left + "px", "top": 70 + "px" });
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
        var table = layui.table;
        var checkStatus = table.checkStatus('test');
        if (checkStatus.data.length == 0) {
            layer.msg("请选择文件");
            return;
        }
        var filepaths = [];
        for (var i = 0; i < checkStatus.data.length; i++) {
            if (checkStatus.data[i].FileUrl) {
                filepaths.push(checkStatus.data[i].FileUrl);

            }
        }
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
})
$(".clearfix").scroll(function () {   //页面加载时，获取滚动条初始高度
    var th = $(this).scrollTop();

    if (th > 60) {
        $(".table-top").addClass("fixednav");
    } else {
        $(".table-top").removeClass("fixednav");
    }

})
function search(s_name, s_date_start, s_date_end, s_type) {
    //if (search) {
    //    $(".cf").addClass("layui-hide");
    //} else {
    //    $(".cf").removeClass("layui-hide");
    //}
    $.ajax({
        type: "Post",
        url: "GetFile",
        data: {
            search: s_name
            , start: s_date_start
            , end: s_date_end
            , ftype: s_type
        },
        success: function (data) {
            var res = JSON.parse(data);
            var table = layui.table;
            if (res.data.length > 50) {
                //表格
                table.render({
                    elem: '#test'
                    , data: res.data
                    , page: {

                        layout: ['count', 'prev', 'page', 'next']
                        , prev: '上一页'
                        , next: '下一页'
                        , count: res.data.length
                        , limit: 50
                        , theme: '#1E9FFF'
                    }


                    , cols: [[
                        { type: 'checkbox' }
                        , { field: 'Name', width: 620, title: '文件名', sort: true, templet: '#fileName' }
                        , { field: 'Size', title: '大小', sort: true }
                        , { field: 'Date', title: '修改日期', sort: true }

                    ]]

                    , done: function (res) {

                        $("#listcount").text("");
                        $("#listcount").text(res.count);
                        if (res.data && res.data.length == 0) {
                            $("#noData").removeClass("layui-hide");
                        } else {
                            $("#noData").addClass("layui-hide");
                        }
                    }
                    , id: "test"
                });
            }
            else {
                //表格
                table.render({
                    elem: '#test'
                    , data: res.data
                    , limit: 50
                    , cols: [[
                        { type: 'checkbox' }
                        , { field: 'Name', width: 620, title: '文件名', sort: true, templet: '#fileName' }
                        , { field: 'Size', title: '大小', sort: true }
                        , { field: 'Date', title: '修改日期', sort: true }

                    ]]

                    , done: function (res) {
                        $("#listcount").text("");
                        $("#listcount").text(res.count);
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
        }
    })

}




