cd .\

# RemoveMigration.ps1

# 啟用錯誤終止
$ErrorActionPreference = "Stop"

try {
    # 顯示選項清單
    # Write-Host "請選擇要使用的Context：" -ForegroundColor Yellow
    # Write-Host "1. CeosHrDbContext" -ForegroundColor Cyan
    # Write-Host "2. ReportDataDbContext" -ForegroundColor Cyan

    # 提示使用者輸入選項
    # $choice = Read-Host "輸入數字選擇"

    # 根據使用者輸入的選項執行操作
    # switch ($choice) {
    #     "1" {
    #         $ContextName = "CeosHrDbContext"
    #     }
    #     "2" {
    #         $ContextName = "ReportDataDbContext"
    #     }
    #     default {
    #         Write-Host "無效的選擇，請選擇 1 或 2" -ForegroundColor Red
    #     }
    # }

    # 構建 dotnet ef migrations remove 指令
    $Command = "dotnet ef migrations remove"

    # OutputPath 有值才加（包含 -o）
    if (-not [string]::IsNullOrWhiteSpace($OutputPath)) {
        $Command += " -o $OutputPath"
    }

    $Command += " -- local-dev skip"

    # 顯示將要執行的指令
    Write-Host "`n將執行以下指令：" -ForegroundColor Yellow
    Write-Host $Command -ForegroundColor Cyan

    # 執行指令
    Invoke-Expression $Command

    Write-Host "`n遷移移除成功！" -ForegroundColor Green
}
catch {
    Write-Host "`n發生錯誤：" $_.Exception.Message -ForegroundColor Red
}

# Pause
