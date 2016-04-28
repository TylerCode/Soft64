﻿# Tray

`Tray`는 OS의 알림 영역에 아이콘을 표시합니다. 보통 컨텍스트 메뉴(context menu)를
같이 사용합니다.

```javascript
const electron = require('electron');
const app = electron.app;
const Menu = electron.Menu;
const Tray = electron.Tray;

var appIcon = null;
app.on('ready', function(){
  appIcon = new Tray('/path/to/my/icon'); // 현재 어플리케이션 디렉터리를 기준으로 하려면 `__dirname + '/images/tray.png'` 형식으로 입력해야 합니다.
  var contextMenu = Menu.buildFromTemplate([
    { label: 'Item1', type: 'radio' },
    { label: 'Item2', type: 'radio' },
    { label: 'Item3', type: 'radio', checked: true },
    { label: 'Item4', type: 'radio' }
  ]);
  appIcon.setToolTip('이것은 나의 어플리케이션 입니다!');
  appIcon.setContextMenu(contextMenu);
});

```

__플랫폼별 한계:__

* Linux에서는 앱 알림 표시기(app indicator)가 지원되면 해당 기능을 사용합니다. 만약
  지원하지 않으면 `GtkStatusIcon`을 대신 사용합니다.
* Linux 배포판이 앱 알림 표시기만 지원하고 있다면 `libappindicator1`를 설치하여
  트레이 아이콘이 작동하도록 만들 수 있습니다.
* 앱 알림 표시기는 컨텍스트 메뉴를 가지고 있을 때만 보입니다.
* Linux에서 앱 표시기가 사용될 경우, `click` 이벤트는 무시됩니다.
* Linux에서 각각 개별 `MenuItem`의 변경을 적용하려면 `setContextMenu`를 다시
  호출해야 합니다. 예를 들면:

```javascript
contextMenu.items[2].checked = false;
appIcon.setContextMenu(contextMenu);
```

이러한 이유로 Tray API가 모든 플랫폼에서 똑같이 작동하게 하고 싶다면 `click` 이벤트에
의존해선 안되며 언제나 컨텍스트 메뉴를 포함해야 합니다.

## Class: Tray

`Tray`는 [EventEmitter][event-emitter]를 상속 받았습니다.

### `new Tray(image)`

* `image` [NativeImage](native-image.md)

전달된 `image`를 이용하여 트레이 아이콘을 만듭니다.

## Events

`Tray` 모듈은 다음과 같은 이벤트를 가지고 있습니다:

**참고:** 몇몇 이벤트는 특정한 플랫폼에서만 작동합니다.

### Event: 'click'

* `event` Event
  * `altKey` Boolean
  * `shiftKey` Boolean
  * `ctrlKey` Boolean
  * `metaKey` Boolean
* `bounds` Object - 트레이 아이콘의 범위
  * `x` Integer
  * `y` Integer
  * `width` Integer
  * `height` Integer

트레이 아이콘이 클릭될 때 발생하는 이벤트입니다.

**참고:** `bounds`는 OS X 와 Windows에서만 작동합니다.

### Event: 'right-click' _OS X_ _Windows_

* `event` Event
  * `altKey` Boolean
  * `shiftKey` Boolean
  * `ctrlKey` Boolean
  * `metaKey` Boolean
* `bounds` Object - 트레이 아이콘의 범위
  * `x` Integer
  * `y` Integer
  * `width` Integer
  * `height` Integer

트레이 아이콘을 오른쪽 클릭될 때 호출 됩니다.

### Event: 'double-click' _OS X_ _Windows_

* `event` Event
  * `altKey` Boolean
  * `shiftKey` Boolean
  * `ctrlKey` Boolean
  * `metaKey` Boolean
* `bounds` Object - 트레이 아이콘의 범위
  * `x` Integer
  * `y` Integer
  * `width` Integer
  * `height` Integer

트레이 아이콘이 더블 클릭될 때 발생하는 이벤트입니다.

### Event: 'balloon-show' _Windows_

풍선 팝업이 보여질 때 발생하는 이벤트입니다.

### Event: 'balloon-click' _Windows_

풍선 팝업이 클릭될 때 발생하는 이벤트입니다.

### Event: 'balloon-closed' _Windows_

풍선 팝업이 시간이 지나 사라지거나 유저가 클릭하여 닫을 때 발생하는 이벤트입니다.

### Event: 'drop' _OS X_

드래그 가능한 아이템이 트레이 아이콘에 드롭되면 발생하는 이벤트입니다.

### Event: 'drop-files' _OS X_

* `event`
* `files` Array - 드롭된 파일의 경로

트레이 아이콘에 파일이 드롭되면 발생하는 이벤트입니다.

### Event: 'drag-enter' _OS X_

트레이 아이콘에 드래그 작업이 시작될 때 발생하는 이벤트입니다.

### Event: 'drag-leave' _OS X_

트레이 아이콘에 드래그 작업이 종료될 때 발생하는 이벤트입니다.

### Event: 'drag-end' _OS X_

트레이 아이콘에 드래그 작업이 종료되거나 다른 위치에서 종료될 때 발생하는 이벤트입니다.

## Methods

`Tray` 모듈은 다음과 같은 메서드를 가지고 있습니다:

**참고:** 몇몇 메서드는 특정 플랫폼에서만 작동합니다.

### `Tray.destroy()`

트레이 아이콘을 즉시 삭제시킵니다.

### `Tray.setImage(image)`

* `image` [NativeImage](native-image.md)

`image`를 사용하여 트레이 아이콘의 이미지를 설정합니다.

### `Tray.setPressedImage(image)` _OS X_

* `image` [NativeImage](native-image.md)

`image`를 사용하여 트레이 아이콘이 눌렸을 때의 이미지를 설정합니다.

### `Tray.setToolTip(toolTip)`

* `toolTip` String

트레이 아이콘의 툴팁 텍스트를 설정합니다.

### `Tray.setTitle(title)` _OS X_

* `title` String

상태바에서 트레이 아이콘 옆에 표시되는 제목 텍스트를 설정합니다.

### `Tray.setHighlightMode(highlight)` _OS X_

* `highlight` Boolean

트레이 아이콘이 클릭됐을 때 아이콘의 배경이 파란색으로 하이라이트 될지 여부를 지정합니다.
기본값은 true입니다.

### `Tray.displayBalloon(options)` _Windows_

* `options` Object
  * `icon` [NativeImage](native-image.md)
  * `title` String
  * `content` String

트레이에 풍선 팝업을 생성합니다.

### `Tray.popUpContextMenu([menu, position])` _OS X_ _Windows_

* `menu` Menu (optional)
* `position` Object (optional) - 팝업 메뉴의 위치
  * `x` Integer
  * `y` Integer

트레이 아이콘의 컨텍스트 메뉴를 팝업시킵니다. `menu`가 전달되면, `menu`가 트레이
메뉴의 컨텍스트 메뉴 대신 표시됩니다.

`position`은 Windows에서만 사용할 수 있으며 기본값은 (0, 0)입니다.

### `Tray.setContextMenu(menu)`

* `menu` Menu

트레이에 컨텍스트 메뉴를 설정합니다.

[event-emitter]: http://nodejs.org/api/events.html#events_class_events_eventemitter
