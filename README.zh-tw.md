### 此專案提供多種語言之README文件
[![Static Badge](https://img.shields.io/badge/lang-en-red)](https://github.com/Unforgettableeternalproject/Digital-Subcurrent/blob/master/README.md) [![Static Badge](https://img.shields.io/badge/lang-zh--tw-yellow)](https://github.com/Unforgettableeternalproject/Digital-Subcurrent/blob/master/README.zh-tw.md)

---

# 數位潛流

### 免責聲明：本專案為遊戲設計課程作業，課程結束後可能不再更新或維護。

---

這是一個以格子為基礎的解謎遊戲，玩家需要移動角色，收集關鍵物品，並到達指定的出口以完成每個關卡。本遊戲結合了邏輯與策略挑戰，適合喜歡思考的玩家。

## 🕹️ 主要特色

- **多樣的關卡設計**：每個關卡都有不同的挑戰與邏輯機制。
- **動態過場效果**：黑屏淡入淡出與進度條為遊戲增添流暢感。
- **物件互動與地圖生成**：物件位置與地圖矩陣自動生成，方便後續擴展。
- **玩家移動與房間轉換**：角色可以在不同房間間無縫切換，體驗完整的遊戲流暢性。

## 🎮 遊戲規則

1. 玩家需要收集 "鑰匙碼 (Keycodes)" 並與 "終端機 (Terminals)" 互動來開啟出口。
2. 遊戲包含移動障礙物 (例如箱子) 的機制，推動障礙物時需注意策略。
3. 推動箱子到錯誤位置可能會導致失敗，需重新嘗試。
4. 玩家需根據步數完成挑戰，時間會根據玩家的移動步數進行推進。

## 🔧 遊戲結構

- **核心邏輯**：
  1. 玩家推動箱子到空地時，該空地會變為可行走區域。
  2. 玩家必須收集所有的 "鑰匙" 並到達指定位置來打開門並離開房間。
  3. 玩家與物件的位置會始終對齊格子的中心，避免出現位置偏移問題。

- **房間切換**：
  玩家通過房間內的門，會進入下一個房間，並觸發過場動畫與地圖更新。

## 🚀 遊戲需求

- **開發環境**：
  - Unity 2022 或更高版本
  - C#

- **依賴項目**：
  1. 進度條控制腳本 `ProgressBarCircle`
  2. 過場效果管理腳本 `TransitionManager`
  3. 地圖生成腳本 `LoadMapInfo`
  4. 關卡載入腳本 `LevelLoader`

## 🛠️ 安裝與執行

1. 下載專案檔案，並使用 Unity 開啟。
2. 確保所有依賴項目已正確導入專案中。
3. 在 Unity 中執行遊戲場景即可開始遊戲。

## 🌟 貢獻

歡迎提交 issue 或 pull request 來改進遊戲！

- 如果有建議或問題，請聯絡開發者。
- 您也可以協助設計新的關卡或改進遊戲功能。

## 📝 聯絡方式

- 開發者：Bernie
- 電子信箱：[![Static Badge](https://img.shields.io/badge/mail-Bernie-blue)
](mailto:ptyc4076@gmail.com) 
- GitHub 項目網址：[GitHub Repository](https://github.com/Unforgettableeternalproject/GridPuzzleGame)
