// Copyright (c) 2016 GitHub, Inc.
// Use of this source code is governed by the MIT license that can be
// found in the LICENSE file.

#include "atom/browser/api/atom_api_system_preferences.h"

#include <map>

#import <Cocoa/Cocoa.h>

#include "atom/common/native_mate_converters/gurl_converter.h"
#include "base/strings/sys_string_conversions.h"
#include "net/base/mac/url_conversions.h"

namespace atom {

namespace api {

namespace {

int g_next_id = 0;

// The map to convert |id| to |int|.
std::map<int, id> g_id_map;

}  // namespace

int SystemPreferences::SubscribeNotification(const std::string& name,
                                             const base::Closure& callback) {
  int request_id = g_next_id++;
  __block base::Closure copied_callback = callback;
  g_id_map[request_id] = [[NSDistributedNotificationCenter defaultCenter]
      addObserverForName:base::SysUTF8ToNSString(name)
      object:nil
      queue:nil
      usingBlock:^(NSNotification* notification) {
        copied_callback.Run();
      }
  ];
  return request_id;
}

void SystemPreferences::UnsubscribeNotification(int request_id) {
  auto iter = g_id_map.find(request_id);
  if (iter != g_id_map.end()) {
    id observer = iter->second;
    [[NSDistributedNotificationCenter defaultCenter] removeObserver:observer];
    g_id_map.erase(iter);
  }
}

v8::Local<v8::Value> SystemPreferences::GetUserDefault(
    const std::string& name, const std::string& type) {
  NSUserDefaults* defaults = [NSUserDefaults standardUserDefaults];
  NSString* key = base::SysUTF8ToNSString(name);
  if (type == "string") {
    return mate::StringToV8(
        isolate(), base::SysNSStringToUTF8([defaults stringForKey:key]));
  } else if (type == "boolean") {
    return v8::Boolean::New(isolate(), [defaults boolForKey:key]);
  } else if (type == "float") {
    return v8::Number::New(isolate(), [defaults floatForKey:key]);
  } else if (type == "integer") {
    return v8::Integer::New(isolate(), [defaults integerForKey:key]);
  } else if (type == "double") {
    return v8::Number::New(isolate(), [defaults doubleForKey:key]);
  } else if (type == "url") {
    return mate::ConvertToV8(
        isolate(), net::GURLWithNSURL([defaults URLForKey:key]));
  } else {
    return v8::Undefined(isolate());
  }
}

bool SystemPreferences::IsDarkMode() {
  NSString* mode = [[NSUserDefaults standardUserDefaults]
      stringForKey:@"AppleInterfaceStyle"];
  return [mode isEqualToString:@"Dark"];
}

}  // namespace api

}  // namespace atom
