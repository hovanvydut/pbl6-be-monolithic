# Homie - INN Findor System - Backend

<img alt="logo.png" height="100" src=".github/img/logo.png" width="100"/>

An INN Findor System web api created by PBL6 Team 🤖

## Features 🔥

- Authentication
- Authorization with Role and Permission
- Create, View, Delete, Edit, Search, Filter, Uptop Rental post
- Review post, create booking to view rental house
- Review evaluation analysis
- User management
- User post management
- Booking management
- Bookmark management
- Statistics
- Payment management (VNPAY)
- Notifications

## Core structure used 🔬

- Very Good Ventures's Boring Structure, check out in this [link](https://verygood.ventures/blog/very-good-flutter-architecture) to learn more about app architecture.
- Multi-package (Mono repo).

## Technologies used 💪

- [flutter_bloc](https://pub.dev/packages/flutter_bloc) for state management solution.
- Service locator using [get_it](https://pub.dev/packages/get_it) and DI via Widget Tree.
- [go_router](https://pub.dev/packages/go_router) for routing solution, deep link.
- Structure templates using [Mason](https://brickhub.dev) brick with [own implementation](https://github.com/dungngminh/mason_bricks) and Very Good Ventures bricks.
- Secure Storage via [flutter_secure_storage](https://pub.dev/packages/flutter_secure_storage).
- [http](https://pub.dev/packages/http) ([customized](https://pub.dev/packages/http_client_handler) wrapper for http session) for api call.

## Design System/UI-UX ️🎨

- [Material Design 3](https://m3.material.io/) (some widgets aren't supported will be customized).
- [Material 3 Dynamic Color](https://m3.material.io/theme-builder#/custom).

## Environment 🚀

This project contains 2 flavors:

- development
- production

To run the desired flavor either use the launch configuration in VSCode/Android Studio or use the following commands with env variables:

```sh
# Development
$ flutter run --flavor development --target lib/main_development.dart --dart-define BASE_URL="your_development_url"
``
# Production
$ flutter run --flavor production --target lib/main_production.dart --dart-define BASE_URL="your_production_url"
```

## Platforms 📦

- Local run by .NET CLI
- Run docker image

### Docker image

<a href='https://play.google.com/store/apps/details?id=me.dungngminh.pbl6_mobile'>
<img alt='Get it on Google Play' src='https://play.google.com/intl/en_us/badges/static/images/badges/en_badge_web_generic.png' width="200"/></a>


## Contributor 🌟

<table>
  <tr>
    <td align="center"><img src="https://avatars.githubusercontent.com/u/65323507?v=4" width="100px;" alt=""/><br /><sub><b>Truong Minh Phuoc</b></sub></a><br /><a href="https://github.com/hovanvydut/pbl6-be-monolithic/commits?author=phuocleoceo" title="Backend Dev">💻</a> 
    <td align="center"><img src="https://avatars.githubusercontent.com/u/54426113?v=4" width="100px;" alt=""/><br /><sub><b>Ho Van Vy</b></sub></a><br /><a href="https://github.com/hovanvydut/pbl6-be-monolithic/commits?author=hovanvydut" title="Backend Dev & Devops">💻🛠</a>
</tr>

</table>