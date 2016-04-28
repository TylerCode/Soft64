﻿# Accelerator

Accelerator는 키보드 단축키를 표현하는 문자열입니다, 여러 혼합키와 키코드를 `+` 문자를
이용하여 결합할 수 있습니다.

예제:

* `CommandOrControl+A`
* `CommandOrControl+Shift+Z`

## 플랫폼에 관련하여 주의할 점

Linux와 Windows에서는 `Command`키가 없으므로 작동하지 않습니다. 대신에
`CommandOrControl`을 사용하면 OS X의 `Command`와 Linux, Windows의 `Control` 모두
지원할 수 있습니다.

`Option` 대신 `Alt`을 사용하는게 좋습니다. `Option` 키는 OS X에만 있으므로
모든 플랫폼에서 사용할 수 있는 `Alt` 키를 권장합니다.

`Super`키는 Windows와 Linux 에서는 `윈도우`키를, OS X에서는 `Cmd`키로 맵핑됩니다.

## 사용 가능한 혼합키

* `Command` (단축어 `Cmd`)
* `Control` (단축어 `Ctrl`)
* `CommandOrControl` (단축어 `CmdOrCtrl`)
* `Alt`
* `Option`
* `AltGr`
* `Shift`
* `Super`

## 사용 가능한 전체 키코드

* `0` 부터 `9` 까지
* `A` 부터 `Z` 까지
* `F1` 부터 `F24` 까지
* `~`, `!`, `@`, `#`, `$`, etc 와 같은 구두점 기호들
* `Plus`
* `Space`
* `Backspace`
* `Delete`
* `Insert`
* `Return` (또는 `Enter`)
* `Up`, `Down`, `Left` 와 `Right`
* `Home` 그리고 `End`
* `PageUp` 그리고 `PageDown`
* `Escape` (단축어 `Esc`)
* `VolumeUp`, `VolumeDown` 그리고 `VolumeMute`
* `MediaNextTrack`, `MediaPreviousTrack`, `MediaStop` 그리고 `MediaPlayPause`
* `PrintScreen`

__키코드는 `단축어`로도 사용할 수 있습니다__
