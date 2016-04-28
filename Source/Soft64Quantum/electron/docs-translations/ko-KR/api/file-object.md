﻿# `File` 객체

DOM의 File 인터페이스는 네이티브 파일을 추상화 합니다. 유저가 직접 HTML5 File API를
이용하여 작업할 때 선택된 파일의 경로를 알 수 있도록 Electron은 파일의 실제 경로를
담은 `path` 속성을 File 인터페이스에 추가하였습니다.

다음 예제는 앱으로 드래그 앤 드롭한 파일의 실제 경로를 가져옵니다:

```html
<div id="holder">
  Drag your file here
</div>

<script>
  var holder = document.getElementById('holder');
  holder.ondragover = function () {
    return false;
  };
  holder.ondragleave = holder.ondragend = function () {
    return false;
  };
  holder.ondrop = function (e) {
    e.preventDefault();
    var file = e.dataTransfer.files[0];
    console.log('File you dragged here is', file.path);
    return false;
  };
</script>
```
