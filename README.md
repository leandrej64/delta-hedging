# Delta Hedging

## Overview

This is an academic delta-hedging project, adapted from the original course project with two main changes:

- **Rebalancing schedule.** The original project supported two rebalancing oracles: on spot variation, or on a fixed period. This version only implements **fixed-period rebalancing**.
- **Pricing engine.** The original relied on a separate pricer library provided for the course. This version uses my own option pricer, [pricer](https://github.com/leandrej64/pricer), instead: the C++ pricer is compiled as a shared library (`.so`) and called from this C# project through a thin C bridge (`extern "C"` API), via P/Invoke.

The primary goal of this project is to **visually observe how well a discretely-rebalanced replicating portfolio tracks an option's theoretical price**, and to highlight the discrepancies with theory introduced by discretization (finite rebalancing frequency instead of continuous hedging). For the precise definition of each option type and its payoff, refer to the [pricer](https://github.com/leandrej64/pricer) project.

## Repository structure

```
hedging/
├── BackTester/        # C# console project (the backtest itself)
├── data/               # option definitions (.json) and simulated market paths (.txt)
├── hedging_data/       # JSON exports of each backtest run
├── figures/            # exported plots
└── hedging_review.ipynb  # notebook used to plot the results
```

### Classes (`BackTester/`)

| Class | File | Role |
|---|---|---|
| `Tester` | `BackTester.cs` | Orchestrates the backtest: steps through time on a fixed grid, calls the pricer at each rebalancing date, and drives the portfolio updates. |
| `Portfolio` | `Portfolio.cs` | Self-financing portfolio bookkeeping: cash (accruing at the risk-free rate between rebalancing dates), asset composition, mark-to-market value, and transaction costs. |
| `NativePricer` | `NativePricer.cs` | P/Invoke bridge to the native `pricer_capi` shared library (pricing and market spot queries). |
| `PricingResult` | `PricingResult.cs` | Managed object exposing a pricing call's result (price, deltas and their standard deviations). |
| `HedgingData` | `HedgingData.cs` | A single time-step's recorded state (time, portfolio value, option price, cumulated transaction costs). |
| `Program.cs` | — | Entry point: wires everything together and exports the run's results to JSON. |

## Results

Three option types were backtested with daily rebalancing: a 5-asset basket option, an Asian option, and a performance ("cliquet") option. Portfolio value is plotted in red, option price in black.

**Basket (5 assets)**

![basket_5d](figures/basket_5d.png)

**Asian**

![asian](figures/asian.png)

**Performance**

![perf](figures/perf.png)

Across all three option types, the replicating portfolio tracks the option's price relatively well — consistent with the expected discretization error of a finitely-rebalanced hedge. The performance option shows a slightly larger residual gap than the basket and Asian options.
