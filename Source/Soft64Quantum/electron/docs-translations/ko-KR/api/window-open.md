﻿# `window.open` 함수

`window.open` 함수가 호출되면 새 창을 생성하고 `url` 페이지를 불러옵니다. 이 창은
지정한 `url`을 로드하여 만들어진 `BrowserWindow`의 새 인스턴스이며 본래 창 객체 대신
페이지의 컨트롤이 제한된 프록시 객체를 반환합니다.

프록시 객체는 브라우저의 웹 페이지 창과 호환될 수 있도록 일부 제한된 표준 기능만 가지고
있습니다. 창의 모든 컨트롤 권한을 가지려면 `BrowserWindow`를 직접 생성해서 사용해야
합니다.

새롭게 생성된 `BrowserWindow`는 기본적으로 부모 창의 옵션을 상속합니다. 이 옵션을
변경하려면 새 창을 열 때 `features` 인자를 지정해야 합니다.

### `window.open(url[, frameName][, features])`

* `url` String
* `frameName` String (optional)
* `features` String (optional)

`BrowserWindowProxy` 클래스의 객체를 반환하는 새로운 윈도우를 생성합니다.

`features` 문자열은 표준 브라우저의 포맷을 따르고 있지만, 각 기능은 `BrowserWindow`의
옵션이어야 합니다.

**참고:** Node 통합 기능은 열린 `window`에서 부모 윈도우가 해당 옵션이
비활성화되어있는 경우 항상 비활성화됩니다.

### `window.opener.postMessage(message, targetOrigin)`

* `message` String
* `targetOrigin` String

부모 윈도우에 메시지를 보냅니다. origin을 특정할 수 있으며 `*`를 지정하면 따로 origin
설정을 사용하지 않습니다.

## Class: BrowserWindowProxy

### `BrowserWindowProxy.blur()`

자식 윈도우의 포커스를 해제합니다.

### `BrowserWindowProxy.close()`

자식 윈도우를 강제로 닫습니다. unload 이벤트가 발생하지 않습니다.

### `BrowserWindowProxy.closed`

자식 윈도우가 닫히면 true로 설정됩니다.

### `BrowserWindowProxy.eval(code)`

* `code` String

자식 윈도우에서 특정 스크립트를 실행합니다.

### `BrowserWindowProxy.focus()`

자식 윈도우에 포커스를 맞춥니다. (창을 맨 앞으로 가져옵니다)

### `BrowserWindowProxy.postMessage(message, targetOrigin)`

* `message` String
* `targetOrigin` String

자식 윈도우에 메시지를 보냅니다. origin을 특정할 수 있으며 `*`를 지정하면 따로 origin
설정을 사용하지 않습니다.

참고로 자식 윈도우의 `window.opener` 객체에는 다른 속성 없이 이 메서드 한 개만
구현되어 있습니다.
