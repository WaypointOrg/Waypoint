"""
Génère un graphique en bar des contribution par contributeur.
"""

import os
import subprocess
from matplotlib import pyplot as plt

def update_repo():
    if os.path.exists(REPO_PATH):
        os.chdir(REPO_PATH)
        os.system("git pull")
    else:
        os.system(f"git clone {CLONE_URL} {REPO_PATH}")
        os.chdir(REPO_PATH)

def switch_to_commit(ref):
    os.system(f"git checkout {ref}")

def get_contributions():
    switch_to_commit("master")
    result = subprocess.check_output("git shortlog -ns", shell=True).decode().rstrip()
    lines = result.split("\n")
    contributors = []
    contributions = []
    for line in lines:
        parts = line.split("\t")
        contributions.append(int(parts[0].rstrip()))
        contributors.append(parts[1])
    return contributors, contributions

CLONE_URL = "https://github.com/DanielRoulin/Waypoint.git"
REPO_PATH =  os.path.join(os.path.dirname(os.path.realpath(__file__)), "Waypoint")

if __name__ == "__main__":
    update_repo()
    contributors, contributions = get_contributions()
    contributors.append("Total")
    contributions.append(sum(contributions))

    axes = plt.subplot(1, 1, 1)
    axes.set_title("Contributions par personnes")
    axes.set_xlabel("Personnes")
    axes.set_ylabel("Commits")
    bars = axes.bar(contributors, contributions)
    bars[0].set_color("blue")
    bars[1].set_color("orange")
    bars[-1].set_color("red")
    plt.show()