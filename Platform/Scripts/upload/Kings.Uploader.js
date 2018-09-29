/// <reference path="Kings.js" />
(function () {
    Kings = window.Kings = function () {

    };
    Kings.Uploader = function () {

    };
    Kings.AjaxJson = function (url, parameter, call, asyncvalue) {
        $.ajax({
            url: url,
            type: "post",
            data: parameter,
            dataType: "json",
            async: asyncvalue == null ? true : false,
            cache: false,
            success: function (data) {
                call(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("操作时发生错误，错误代码：" + XMLHttpRequest.status);
            }
        });
    };
    Kings.Uploader.cloneObject = function (NewObject, OldObject) {
        function clonePrototype() { }
        clonePrototype.prototype = NewObject;
        var obj = new clonePrototype();
        for (var ele in obj) {
            if (typeof (obj[ele]) == "object")
                Kings.ObjtoObj(obj[ele], OldObject[ele]);
            else
                OldObject[ele] = obj[ele];
        }
    }
    Kings.Uploader.prototype.item = {
        disableGlobalDnd: true,
        paste: document.body,
        pick: "picker",
        accept: null,
        thumb: {
            width: 110,
            height: 110,
            quality: 70,
            allowMagnify: true,
            crop: true,
            type: 'image/jpeg'
        },
        compress: {
            width: 1600,
            height: 1600,
            quality: 90,
            allowMagnify: false,
            crop: false,
            preserveHeaders: true,
            noCompressIfLarger: false,
            compressSize: 0
        },
        auto: true,
        prepareNextFile: true,
        chunked: true,
        chunkSize: 2097152,
        chunkRetry: 2,
        threads: 3,
        //formData: {}
        fileVal: "file",
        formData: {},

        //other.Parameter
        startcheckchunk: true,//产前检查是否开启
        startcheckserver: null,
        listchunks: {},
        filish:false,
        valimd5url: null
    };
    Kings.Uploader.prototype.init = function () {
        WebUploader.Uploader.register({
            "before-send": "beforeSend"
        }, {
            beforeSend: function (block) {
                var task = new $.Deferred();
                var listchunk = this.owner.options.listchunks[block.file.name];
                if (this.owner.options.filish)
                {
                    task.reject();
                }
                else
                {
                    if (listchunk == null || listchunk == undefined || listchunk.length == 0) {
                        task.resolve();
                    }
                    else {
                        var hasrect = false;
                        for (var i = 0; i < listchunk.length; i++) {
                            if (block.chunk == listchunk[i]) {
                                task.reject();
                                hasrect = true;
                                listchunk.splice(i, 1);
                                break;
                            }
                        }
                        if (!hasrect) {
                            task.resolve();
                        }
                    }
                }
                
                return $.when(task);
            }
        });
    };
    Kings.Uploader.prototype.createImage = function (parameter, eventparameter) {
        Kings.Uploader.cloneObject(parameter, this.item);
        this.item.accept = {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        };
        this.item.chunked = false;
        this.init();
        var uploader = WebUploader.create(this.item);
        Kings.Uploader.bind(eventparameter, uploader,this);
        return uploader;
    };
    Kings.Uploader.prototype.createFile = function (parameter, eventparameter) {
        
        Kings.Uploader.cloneObject(parameter, this.item);
        this.item.accept = {
            title: 'Files',
            extensions: 'png,xls,xlsx,doc,docx,ppt,pptx,pdf,txt,zip,wps,dps,et,jpg,bmp,rar',
            mimeTypes: '.png,.xls,.xlsx,.doc,.docx,.ppt,.pptx,.pdf,.txt,.zip,.wps,.dps,.et,.jpg,.bmp,.rar'
        };
        this.init();
        var uploader = WebUploader.create(this.item);
        Kings.Uploader.bind(eventparameter, uploader,this);
        return uploader;
    };
    Kings.Uploader.bind = function (eventparameter, uploader,kingsuploader) {
        if (kingsuploader.item.startcheckchunk) {
            uploader.on("uploadStart", function (file) {
                var myp = $("#filepath").val();
                var formData = {
                    path: myp,
                    size: file.size,
                    startcheckchunk: true,
                    loaded: file.loaded,
                    filename: file.name,
                    id: file.id,
                    ext: file.ext,
                    lastModifiedDat: file.lastModifiedDat,
                    chunksize: kingsuploader.item.chunkSize,
                    md5: uploader.options.formData.md5
                };
                
                Kings.AjaxJson(kingsuploader.item.startcheckserver || kingsuploader.item.server, formData,
                    function (result) {
                        uploader.options.listchunks[file.name] = result.ChunkNum;
                        if(result.Filish==true)
                        {
                            uploader.options.filish = true;
                        }
                        //console.log(result.Message);
                    }, false);//ajax注意使用同步
            });
        };
        for (var ev in eventparameter) {
            uploader.on(ev, eventparameter[ev]);
        }
    }
})();

